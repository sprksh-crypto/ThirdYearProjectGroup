import sys
import matplotlib.pyplot as plt

if len(sys.argv) != 2:
    raise Exception("Locate to directory in terminal and type \"python3 plotter.py filename.txt\"")

file_name = sys.argv[1]
data = []
with open(file_name) as f:
    data = f.readlines()
    data = [float(x[:-1]) for x in data]

plt.figure()
plt.suptitle(file_name)
plt.xlabel("Time step")
plt.ylabel("Distance (Unity units)")
plt.plot(data)
plt.show()
