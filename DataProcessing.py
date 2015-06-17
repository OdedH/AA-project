from sklearn import datasets
from sklearn import feature_extraction
from sklearn import preprocessing
from sklearn import svm
from sklearn import grid_search
from sklearn import cross_validation
from sentences import *
from sklearn.feature_selection import VarianceThreshold
from removeRedundantFiles import *

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

# TODO: add features here

raw_documents = raw_bunch['data']
lenMatrix = buildCSRLengths();
document_term_matrix = vectorizer.fit_transform(raw_documents)

print(lenMatrix.shape);
print(document_term_matrix.shape)
document_term_matrix = csr_append(lenMatrix, document_term_matrix);
print("Shape before feature selection");
print(document_term_matrix.shape)
sel = VarianceThreshold(threshold=(.8 * (1 - .8)));
new_x = sel.fit_transform(document_term_matrix);
print("Shape after feature selection");
print (new_x.shape)


print("     Done!")
print("     scaling ...")
scaler = preprocessing.StandardScaler(copy=True, with_mean=False, with_std=True).fit(document_term_matrix)
# We are using sparse matrix so we have to define with_mean=False
# Scaler can be used later for new objects
document_term_matrix_scaled = scaler.transform(document_term_matrix)
print("     Done!")
print("Done!")

# STEP 2 - Tries

parameters = {'kernel':('linear', 'rbf'), 'gamma':[1e-1, 1, 1e1], 'cache_size':[2048]}
print("Done2!")
svc=svm.SVC()
print("Done3!")
clf=grid_search.GridSearchCV(svc,parameters)
print("Done4!")

scores = cross_validation.cross_val_score(clf,new_x, raw_bunch['target'], scoring=None,
                                                  cv=2, n_jobs=1, verbose=0,
                                                  fit_params=None, pre_dispatch='2*n_jobs')
print(scores)


