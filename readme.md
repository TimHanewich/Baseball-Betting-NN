# Predicting MLB Betting Lines with Neural Networks


## Model Downloads
|Name|Parameters|Description|
|-|-|-|
|model4||Trained on ~5,300 examples|
|model5|378,274|Trained on 5,473 examples|

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