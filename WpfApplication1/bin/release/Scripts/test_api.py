
import sys
def main(argv):
    if "a" in argv:
        a()
        if "b" in argv:
            b()

def a():
    print "test"

def b():
    print "test 2 :)"

if __name__ == '__main__':
    main(sys.argv)
