# Predicting MLB Betting Lines with Neural Networks


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

MLB Scoreboard on ESPN: https://www.espn.com/mlb/scoreboard

## Baseball Game State
- Away Runs
- Home Runs
- inning
- Batting Team (0 = away, 1 = home)
- number of outs
- balls
- strikes
- Man on first?
- Man on second?
- Man on third?

### Additional, for neural network:
- Away Team Winning Percentage
- Home Team Winning Percentage

## Need to tweak
- When a batter walks, ESPN will mark it with 4 balls in the count AND a man on second temporarily. If there are 4 balls and a man is on, count it as 0 balls.