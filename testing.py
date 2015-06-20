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
from sklearn.cross_validation import train_test_split
from sklearn import metrics

RemoveFiles()
print("Importing data from folder...")
# here we create a Bunch object ['target_names', 'data', 'target', 'DESCR', 'filenames']
raw_bunch = datasets.load_files('./data', description=None, categories=None, load_content=True,
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
lenMatrix = buildCSRLengths()
document_feature_matrix = vectorizer.fit_transform(raw_documents)

document_feature_matrix = csr_append(lenMatrix, document_feature_matrix)

print("     Done!")
print("     scaling ...")
scaler = preprocessing.StandardScaler(copy=True, with_mean=False, with_std=True).fit(document_feature_matrix)
# We are using sparse matrix so we have to define with_mean=False
# Scaler can be used later for new objects
document_term_matrix_scaled = scaler.transform(document_feature_matrix)
print("     Done!")
print("Done!")

# sel = VarianceThreshold(threshold=(.8 * (1 - .8)))
# document_feature_matrix = sel.fit_transform(document_feature_matrix)

# fs = feature_selection.SelectPercentile(feature_selection.chi2, percentile=80)
# document_feature_matrix = fs.fit_transform(document_feature_matrix, raw_bunch['target'])

# STEP 2 - Tries
X_train, X_test, y_train, y_test = train_test_split(document_feature_matrix,
                                                    raw_bunch['target'], test_size=0.5, random_state=42)

parameters = parameters = [{'kernel': ['rbf'], 'gamma': [1e-1, 1, 1e1, 1e-3, 1e-4],
                            'C': [1, 10, 100, 1000]},
                           {'kernel': ['linear'], 'C': [1, 10, 100, 1000]},
                           {'kernel': ['poly'], 'degree': [1, 2, 3, 4, 5, 10]}
                           ]
print("Building a classifier...")
print("    parameters tuning...")
clf = grid_search.GridSearchCV(svm.SVC(C=1), parameters, cv=10)
clf.fit(X_train, y_train)
print("Done!")

print("score test cv:")
scores = cross_validation.cross_val_score(clf, X_test, y_test, scoring=None,
                                          cv=10, n_jobs=1, verbose=0,
                                          fit_params=None, pre_dispatch='2*n_jobs')
print(scores)

print("Best parameters set found on development set:")
print()
print(clf.best_params_)
print()
print("Grid scores on development set:")
print()
for params, mean_score, scores in clf.grid_scores_:
    print("%0.3f (+/-%0.03f) for %r"
          % (mean_score, scores.std() * 2, params))
print()

print("Detailed classification report:")
print()
print("The model is trained on the full development set.")
print("The scores are computed on the full evaluation set.")
print()
y_true, y_pred = y_test, clf.predict(X_test)
print(metrics.classification_report(y_true, y_pred))
print()
