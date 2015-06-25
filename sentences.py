import fnmatch, os
import numpy as np
import scipy.sparse as sp
from scipy.sparse import csr_matrix


def csr_append(a, b):
    return sp.hstack((a, b), format='csr');


def buildCSRLengths(path):
    corpSentLen = []
    corpWordLen = []
    max_sent_len = 0
    max_word_len = 0

    for root, dirnames, filenames in os.walk(path):
        for filename in fnmatch.filter(filenames, '*.txt'):
            lengths = {};
            fileStream = open(os.path.join(root, filename), 'r')
            content = str(fileStream.readlines())
            a = root.split("\\")
            if (len(a) > 1):
                splitted = content.split(".")
                for sen in splitted:
                    words = sen.split()
                    for word in words:
                        if len(word) > max_word_len:
                            max_word_len = len(word)
                    if len(words) > max_sent_len:
                        max_sent_len = len(words)

    for root, dirnames, filenames in os.walk(path):
        for filename in fnmatch.filter(filenames, '*.txt'):
            sentLengths = [0.0] * max_sent_len
            wordLengths = [0.0] * max_word_len
            fileStream = open(os.path.join(root, filename), 'r')
            content = str(fileStream.readlines())
            a = root.split("\\");
            if (len(a) > 1):
                splitted = content.split(".")
                for sen in splitted:
                    words = sen.split()
                    for word in words:
                        wordLengths[len(word) - 1] += 1.0
                    sentLengths[len(words) - 1] += 1.0
            corpSentLen.append(sentLengths)
            corpWordLen.append(wordLengths)
    lensCsr = csr_matrix(corpSentLen)

    wordsCsr = csr_matrix(corpWordLen)

    return [csr_append(lensCsr, wordsCsr), [max_sent_len, max_word_len]];
