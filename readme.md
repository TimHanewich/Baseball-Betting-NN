# Predicting MLB Betting Lines with Neural Networks
This project leverages Tensorflow's Keras API to compile, train, and use a neural network to predict betting lines in baseball game. Training data is gathered from ESPN (the *state*, or inputs) and DraftKings (the prediction, the outputs).

Simply put - for any point in a theoretical game of baseball, this model predicts what the standard betting lines for the game should be, and thus, can predict a winner.

![Baseball Betting Line Prediction Engine](https://i.imgur.com/okWJ3A9.png)


## How to run the model
1. Download and unzip one of the pre-trained models in the [download section below](#model-downloads).
2. Replace the value of the `nn_model_path` variable in the [ui.py](./nn_model/ui.py) with the path of the folder *within* the unzipped folder.
3. Install required dependencies:
    1. **tensorflow**: `python -m pip install tensorflow`
4. Run [ui.py](./nn_model/ui.py) to run the model and GUI!

## Model Inputs (the State)
The model considers the following 16 inputs when predicting betting lines, in this order: 
- Away team record, as a percentage (i.e. 0.8 if the team is 8-2, meaning the team won 8 out of their 10 games total)
- Home team record
- Number of runs the away team has
- Number of runs the home team has
- Number of hits the away team has
- Number of hits the home team has
- Number of errors the away team has
- Number of errors the home team has
- The current inning - i.e. 1.0 for first inning, 2.0 for second, etc. And "0.0" would mean the game is yet to be started and the betting lines are a pre-game line.
- Top or bottom of inning? Top = 0.0, Bottom = 1.0
- Number of outs
- Number of balls in the batter's count
- Number of strikes in the batter's count
- Is there a runner on first base? No = 0.0, Yes = 1.0
- Is there a runner on second base? No = 0.0, Yes = 1.0
- Is there a runner on third base? No = 0.0, Yes = 1.0

## Model Outputs (the Prediction)
The model predicts four distinct betting lines, in the following order. You can read more about what each of these lines mean [here](https://sportsbook.draftkings.com/help/how-to-bet/baseball-betting-guide).
- The *Run Line* - the "point spread" between the two teams.
- The *Total Line* - the prediction for what the under/over would be for the combined number of runs in the game. 
- The *Away Team's Money Line*
- The *Home Team's Money Line*
Using the odds above, particularly the money lines, we can use these to calculate the implied win probability for either team.

## Model Downloads
|Name|Parameters|Description|
|-|-|-|
|[model4](https://timhmsft.blob.core.windows.net/downloadable/model4.zip?sp=r&st=2023-04-20T15:19:20Z&se=2999-04-20T23:19:20Z&spr=https&sv=2021-12-02&sr=b&sig=AVQ9fkDrzJCz3p7XPqYQ%2Fr6lSL5o6btCZc2Mj22KnGM%3D)|7,469|Trained on ~5,300 examples|
|[model5](https://timhmsft.blob.core.windows.net/downloadable/model5.zip?sp=r&st=2023-04-20T15:19:53Z&se=2999-04-20T23:19:53Z&spr=https&sv=2021-12-02&sr=b&sig=3X277mvDT0%2Fp3mA1jC476jeH6QNH8sX7HcCnnLLRmwE%3D)|378,274|Trained on 5,473 examples|

## Training Data Downloads
These are `.jsonl` files. Each line is self-contained JSON object with both the state (game scenario) and real-world observed betting line information.
|Number of Examples|
|-|
|[5,473](https://timhmsft.blob.core.windows.net/downloadable/db9jfwejio1h2ohfdsf.jsonl?sp=r&st=2023-04-20T15:08:57Z&se=2999-04-20T23:08:57Z&spr=https&sv=2021-12-02&sr=b&sig=6Z9yJ5P077Q7kxbhafJZ4v3CtmsFAqSuBF%2FrOZCzhng%3D)|

## In this Repo
This repo contains the following programs:
- [A program for capturing training data from ESPN and DraftKings, written in .NET 7](./data_capture/)
- [A python script to assemble, compile, train, and save a tensorflow keras neural network](./nn_model/train.py)
- [A python program to leverage a pre-trained model to allow you to predict for various scenarios, leveraging a pre-trained model](./nn_model/ui.py)

## Future Areas of Improvement
- When a batter walks, ESPN will mark it with 4 balls in the count AND a man on second temporarily. If there are 4 balls and a man is on, count it as 0 balls.