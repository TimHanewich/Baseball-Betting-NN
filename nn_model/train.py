import tensorflow as tf
import numpy
import random
import json



inputs:tf.keras.layers.Dense = tf.keras.layers.Input(16)
h1:tf.keras.layers.Dense = tf.keras.layers.Dense(350, "relu")
h2:tf.keras.layers.Dense = tf.keras.layers.Dense(400, "relu")
h3:tf.keras.layers.Dense = tf.keras.layers.Dense(300, "relu")
h4:tf.keras.layers.Dense = tf.keras.layers.Dense(250, "relu")
h5:tf.keras.layers.Dense = tf.keras.layers.Dense(120, "relu")
h6:tf.keras.layers.Dense = tf.keras.layers.Dense(50, "relu")
outputs:tf.keras.layers.Dense = tf.keras.layers.Dense(4)

model = tf.keras.Sequential()
model.add(inputs)
model.add(h1)
model.add(h2)
model.add(h3)
model.add(h4)
model.add(h5)
model.add(h6)
model.add(outputs)

model.compile("adam", "mean_squared_error")

print("Preparing training data...")
x_train = []
y_train = []
f = open(r"C:\Users\timh\Downloads\tah\nn\db2.jsonl")
while True:
    line = f.readline()
    if not line:
        print("BREAKING!")
        break
    elif line != "":
        o = json.loads(line)
        inputs = o["State"]
        outputs = o["Prediction"]
        x_train.append(inputs)
        y_train.append(outputs)

# close
print("Closing file... ", end="")
f.close()
print("Closed!")

print("X examples: " + str(len(x_train)))
print("Y examples: " + str(len(y_train)))

# Train
i = numpy.array(x_train)
o = numpy.array(y_train)
print("Training... ")
model.fit(i, o, epochs=250, verbose=True)
print("complete!")

# save
model.save(r"C:\Users\timh\Downloads\tah\nn\nn_model\models\model7")
