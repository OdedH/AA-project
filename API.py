import os
from sklearn import datasets
from sklearn import feature_extraction
from sklearn import preprocessing
from sklearn import svm
from sklearn import grid_search
from sklearn import cross_validation
from sentences import *
from sklearn.feature_selection import VarianceThreshold
from removeRedundantFiles import *
from sklearn import feature_selection
import numpy as np
import sys
import dill as pickle
import matplotlib.pyplot as plt
from matplotlib import figure
from pylab import savefig
# import marshal
# import type

# Serialization Initiating - structure
# if os.stat("serialization.p").st_size != 0:
#     classifiers = pickle.load(open('serialization.p', 'rb'))
# else:
#     classifiers = []

# # Serialization Initiating - functions
# if os.stat("serialization.m").st_size != 0:
#     functions_for_classifiers = marshal.load(open('serialization.m', 'rb'))
# else:
#     functions_for_classifiers = []

def csr_append(a, b):
    return sp.hstack((a, b), format='csr');

def buildLengthsForFile(path, new_obj, max_sent_len, max_word_len):
    sentLengths = [0.0] * max_sent_len
    wordLengths = [0.0] * max_word_len
    fileStream = open(path, 'r')
    content = str(fileStream.readlines())
    splitted = content.split(".")
    for sen in splitted:
        words = sen.split()
        for word in words:
            if len(word) <= max_word_len:
                wordLengths[len(word) - 1] += 1.0
        if len(words) <= max_sent_len:
            sentLengths[len(words) - 1] += 1.0
    return csr_append(new_obj, csr_matrix(sentLengths+wordLengths));


def fit_object(path, classifier):
    with open(path, "r") as new_obj:
        new_obj = new_obj.read().replace('\n', '')
    new_obj = unicode(new_obj, "utf-8")
    new_obj = (classifier[1][0])([new_obj])
    new_obj = buildLengthsForFile(path, new_obj, classifier[1][1][0],
                                  classifier[1][1][1])
    new_obj = classifier[1][2](new_obj)
    new_obj = classifier[1][3](new_obj)
    new_obj = classifier[1][4](new_obj)
    return new_obj


def classify_object(path, classifier_num):
    print("Unpickling...")
    classifier = pickle.load(open(str(classifier_num) + ".p", 'rb'))
    print("Done...")
    return classifier[2][classifier[0].predict(fit_object(path, classifier))]


def build_classifier(path="./data", speed=0, feature_percent=100, feature_var=.8):
    classifier_num = 0
    while os.path.isfile(str(classifier_num) + ".p"):
        classifier_num += 1
    open((str(classifier_num) + ".p"), 'a')

    fit_functions = []
    RemoveFiles()
    print("Importing data from folder...")
    # here we create a Bunch object ['target_names', 'data', 'target', 'DESCR', 'filenames']
    raw_bunch = datasets.load_files(path, description=None, categories=None, load_content=True,
                                    shuffle=True, encoding='utf-8', decode_error='replace')
    print("Done!")

    print("Processing text to data...")
    print("     extracting features...")
    vectorizer = feature_extraction.text.CountVectorizer(input=u'content', encoding=u'utf-8', decode_error=u'strict',
                                                         strip_accents=None, lowercase=True,
                                                         preprocessor=None, tokenizer=None,
                                                         # trying with enabled parameters
                                                         stop_words=None,  # we count stop words
                                                         token_pattern=r'(?u)\b\w\w+\b',
                                                         ngram_range=(1, 3),  # 1grams to 3grams
                                                         analyzer=u'word', max_df=1.0, min_df=1, max_features=None,
                                                         vocabulary=None, binary=False,
                                                         dtype='f')

    raw_documents = raw_bunch['data']
    document_feature_matrix = vectorizer.fit_transform(raw_documents)
    fit_functions.append(vectorizer.transform)

    temp = buildCSRLengths(path)
    lenMatrix = temp[0]
    fit_functions.append(temp[1])

    document_feature_matrix = csr_append(lenMatrix, document_feature_matrix)
    print("     Done!")
    print("     scaling ...")
    scaler = preprocessing.StandardScaler(copy=True, with_mean=False, with_std=True).fit(document_feature_matrix)
    # We are using sparse matrix so we have to define with_mean=False
    # Scaler can be used later for new objects
    document_feature_matrix = scaler.transform(document_feature_matrix)
    fit_functions.append(scaler.transform)
    print("     Done!")
    print("     selecting features ...")
    sel = VarianceThreshold(threshold=(feature_var * (1 - feature_var)))
    document_feature_matrix = sel.fit_transform(document_feature_matrix)
    fit_functions.append(sel.transform)
    fs = feature_selection.SelectPercentile(feature_selection.chi2, percentile=feature_percent)
    document_feature_matrix = fs.fit_transform(document_feature_matrix, raw_bunch['target'])
    # TODO check other parameters for feature selection
    # TODO  According to sci-kit, raw_bunch['target'] is optional.
    # TODO I don"t know how it is possible but maybe we should check that.
    fit_functions.append(fs.transform)
    print("     Done!")
    print("Done!")
    print("Building a classifier...")
    print("    parameters tuning...")
    parameters = []
    cv = 1
    if speed is 0:
        parameters = [{'kernel': ['rbf'], 'gamma': [1e-1, 1, 1e1, 1e-3, 1e-4],
                       'C': [1, 10, 100, 1000]},
                      {'kernel': ['linear'], 'C': [1, 10, 100, 1000]}]
        cv = 3
    if speed is 1:
        parameters = [{'kernel': ['rbf'], 'gamma': [1e-1, 1, 1e1, 1e-3, 1e-4],
                       'C': [1, 10, 100, 1000]},
                      {'kernel': ['linear'], 'C': [1, 10, 100, 1000]},
                      {'kernel': ['poly'], 'degree': [1, 2, 3, 4, 5, 10]}
                      ]
        cv = 10
    clf = grid_search.GridSearchCV(svm.SVC(C=1), parameters, cv=cv)
    clf.fit(document_feature_matrix, raw_bunch['target'])
    estimator = clf.best_estimator_
    estimator.fit(document_feature_matrix, raw_bunch['target'])
    print("Done!")
    print("Training...")
    estimator = clf.best_estimator_
    estimator.fit(document_feature_matrix, raw_bunch['target'])
    print("Done!")
    new_classifier = ([estimator, fit_functions, raw_bunch['target_names']])
    # functions_for_classifiers.append(fit_functions)
    # open('serialization.p', 'w').close()
    print("Pickling...")
    pickle.dump(new_classifier, open((str(classifier_num) + ".p"), 'wb'))
    print("Done!")
    # marshal.dump(functions_for_classifiers, open('serialization.m', 'wb'))
    return classifier_num


def delete_classifier(classifier_num):
    os.remove((str(classifier_num) + ".p"))

def build_histogram(path="./data", name="hist"):

    # here we create a Bunch object ['target_names', 'data', 'target', 'DESCR', 'filenames']
    raw_bunch = datasets.load_files(path, description=None, categories=None, load_content=True,
                                    shuffle=True, encoding='utf-8', decode_error='replace')
    quantities = {author: 0 for author in list(raw_bunch['target_names'])}
    for i in list(raw_bunch['target']):
        quantities[list(raw_bunch['target_names'])[i]]+=1
    plt.figure(figsize=(17, 7), dpi=80, facecolor='w', edgecolor='k')
    plt.bar(range(len(quantities)), quantities.values(), align='center')
    plt.xticks(range(len(quantities)), quantities.keys())
    savefig(name + '.png')

if len(sys.argv) != 0:
    if str(sys.argv[1]) == "classify_object":
        print classify_object(sys.argv[2], int(sys.argv[3]))
    if sys.argv[1] == "build_classifier":
        print build_classifier(sys.argv[2], int(sys.argv[3]), float(sys.argv[4]), float(sys.argv[5]))
    if sys.argv[1] == "delete_classifier":
        delete_classifier(int(sys.argv[2]))
    if sys.argv[1] == "build_histogram":
        build_histogram(sys.argv[2], sys.argv[3])
