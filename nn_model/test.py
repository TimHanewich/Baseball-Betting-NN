import tensorflow as tf
import numpy
import json

model:tf.keras.Model = tf.keras.models.load_model(r"C:\Users\timh\Downloads\tah\nn\nn_model\models\model1")

f = open(r"C:\Users\timh\Downloads\tah\nn\validation.jsonl")
while True:
    line = f.readline()
    if not line:
        print("BREAKING!")
        break
    elif line != "":
        o = json.loads(line)
        inputs = o["State"]
        outputs = o["Prediction"]
        
        _i = numpy.array([inputs])
        _o = numpy.array([outputs])

        prediction = model.predict(_i, verbose=False)[0]
        print(str(prediction) + "   Correct: " + str(outputs))
f.close()