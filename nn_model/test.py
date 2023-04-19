import tensorflow as tf
import numpy
import json


f = open(r"C:\Users\timh\Downloads\tah\nn\nn_model\sample_state.json")
content:str = f.read()
obj = json.loads(content)

# prepare state
state = []
state.append(obj["AwayTeamWinningRecord"])
state.append(obj["HomeTeamWinningRecord"])
state.append(obj["AwayTeamRuns"])
state.append(obj["HomeTeamRuns"])
state.append(obj["AwayTeamHits"])
state.append(obj["HomeTeamHits"])
state.append(obj["AwayTeamErrors"])
state.append(obj["HomeTeamErrors"])
state.append(obj["Inning"])
state.append(obj["BattingTeam"])
state.append(obj["Outs"])
state.append(obj["Balls"])
state.append(obj["Strikes"])
state.append(float(obj["ManOnFirst"]))
state.append(float(obj["ManOnSecond"]))
state.append(float(obj["ManOnThird"]))

# convert all to float
state_floats = []
for item in state:
    state_floats.append(float(item))
state = state_floats

model:tf.keras.Model = tf.keras.models.load_model(r"C:\Users\timh\Downloads\tah\nn\nn_model\models\model2")
x = numpy.array([state])

predictions = model.predict(x)[0]

print("Run Line: " + str(predictions[0]))
print("Total Line: " + str(predictions[1]))
print("Away Team Money Line: " + str(predictions[2]))
print("Home Team Money Line: " + str(predictions[3]))
