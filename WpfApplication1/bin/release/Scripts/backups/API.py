import pickle
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


# Serialization Initiating
print("start")
if int(sys.argv[1]) == 1:
    print("start_full")
    classifiers = pickle.load(open('serialization.p', 'rb'))
    print("middle_full")

else:
    print("start_empty")
    classifiers = []

def fit_object(path, classifier_num):
    with open(path, "r") as new_obj:
        new_obj = new_obj.read().replace('\n', '')
    new_obj = classifiers[classifier_num][1][0](new_obj)
    # TODO add here changes from sentences
    new_obj = classifiers[classifier_num][1][1](new_obj)
    new_obj = classifiers[classifier_num][1][2](new_obj)
    return new_obj


def classify_object(path, classifier_num):
    return classifiers[2][classifiers[classifier_num][0].predict(fit_object(path, classifier_num))]


def build_classifier(path="./data", speed=0, feature_percent=1, feature_var=.8):
    print("Starting")
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
    lenMatrix = buildCSRLengths()[0]
    document_feature_matrix = vectorizer.fit_transform(raw_documents)

    fit_functions.append(vectorizer.transform)

    document_feature_matrix = csr_append(lenMatrix, document_feature_matrix)
    print("     Done!")
    print("     scaling ...")
    scaler = preprocessing.StandardScaler(copy=True, with_mean=False, with_std=True).fit(document_feature_matrix)
    # We are using sparse matrix so we have to define with_mean=False
    # Scaler can be used later for new objects
    document_term_matrix_scaled = scaler.transform(document_feature_matrix)
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
    print("Done!")

    classifiers.append([estimator, fit_functions, raw_bunch['target_names']])

    pickle.dump(classifiers, open('serialization.p', 'wb'))

    return len(classifiers) - 1


if len(sys.argv) != 0:
    if str(sys.argv[2]) == "classify_object":
        print classify_object(sys.argv[3], int(sys.argv[3]))
    if sys.argv[2] == "build_classifier":
        print build_classifier(sys.argv[3], float(sys.argv[4]), float(sys.argv[5]), float(sys.argv[6]))

