
import fnmatch, os

def RemoveFiles():
    for root, dirnames, filenames in os.walk('./data'):
        for filename in fnmatch.filter(filenames, '*'):
            if not filename.endswith('.txt') and not filename.endswith('.py'):
                os.remove(os.path.join(root, filename))