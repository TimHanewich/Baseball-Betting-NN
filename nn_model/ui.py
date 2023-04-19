##### SETTINGS #####
nn_model_path = r"C:\Users\timh\Downloads\tah\nn\nn_model\models\model2"
####################




from tkinter import *
import tensorflow as tf
import numpy



# load the neural network into memory
print("Loading model... ")
model:tf.keras.Model = tf.keras.models.load_model(nn_model_path)
print("Model loaded!")



# button states
man_on_first:bool = False
man_on_second:bool = False
man_on_third:bool = False
ball_count = 0
strike_count = 0
out_count = 0
bottom_top_inning = False # False = top of inning (away team batting), True = bottom of inning (home team batting)







root = Tk()
canvas = Canvas(root, width=600, height=600)
canvas.pack()

second_base = canvas.create_polygon(300, 0, 350, 50, 300, 100, 250, 50, fill='gray', outline='black')
first_base = canvas.create_polygon(350, 50, 400, 100, 350, 150, 300, 100, fill='gray', outline='black')
third_base = canvas.create_polygon(250, 50, 300, 100, 250, 150, 200, 100, fill='gray', outline='black')

# create balls, strikes, outs text
canvas.create_text(150, 200, text="Balls")
canvas.create_text(150, 240, text="Strikes")
canvas.create_text(150, 280, text="Outs")

# create balls
balls_0 = canvas.create_rectangle(200, 188, 225, 188 + 25, fill="light blue")
balls_0_text = canvas.create_text(200 + 12.5, 188 + 12.5, text="0")
balls_1 = canvas.create_rectangle(225, 188, 250, 188 + 25, fill="light blue")
balls_1_text = canvas.create_text(225 + 12.5, 188 + 12.5, text="1")
balls_2 = canvas.create_rectangle(250, 188, 275, 188 + 25, fill="light blue")
balls_2_text = canvas.create_text(250 + 12.5, 188 + 12.5, text="2")
balls_3 = canvas.create_rectangle(275, 188, 300, 188 + 25, fill="light blue")
balls_3_text = canvas.create_text(275 + 12.5, 188 + 12.5, text="3")

# create strikes
strikes_0 = canvas.create_rectangle(200, 226, 200 + 25, 226 + 25, fill="pink")
strikes_0_text = canvas.create_text(200 + 12.5, 226 + 12.5, text="0")
strikes_1 = canvas.create_rectangle(225, 226, 225 + 25, 226 + 25, fill="pink")
strikes_1_text = canvas.create_text(225 + 12.5, 226 + 12.5, text="1")
strikes_2 = canvas.create_rectangle(250, 226, 250 + 25, 226 + 25, fill="pink")
strikes_2_text = canvas.create_text(250 + 12.5, 226 + 12.5, text="2")

# create outs
outs_0 = canvas.create_rectangle(200, 267, 200 + 25, 267 + 25, fill="orange")
outs_0_text = canvas.create_text(200 + 12.5, 267 + 12.5, text="0")
outs_1 = canvas.create_rectangle(225, 267, 225 + 25, 267 + 25, fill="orange")
outs_1_text = canvas.create_text(225 + 12.5, 267 + 12.5, text="1")
outs_2 = canvas.create_rectangle(250, 267, 250 + 25, 267 + 25, fill="orange")
outs_2_text = canvas.create_text(250 + 12.5, 267 + 12.5, text="2")

# current inning
canvas.create_text(350, 200, text="Inning")
current_inning = Entry(root)
current_inning.place(x=375, y=191)

# top or bottom (batting team)
inning_top = canvas.create_rectangle(350, 225, 420, 250, fill="light gray")
inning_top_text = canvas.create_text(350 + 35, 225 + 12.5, text="Top")
inning_bottom = canvas.create_rectangle(420, 225, 490, 250, fill="light gray")
inning_bottom_text = canvas.create_text(420 + 35, 225 + 12.5, text="Bottom")


# Away + Home Team Runs, Hits, and Errors, and record
canvas.create_text(120, 350, text="Away")
canvas.create_text(120, 390, text="Home")
canvas.create_text(200, 325, text="Runs")
canvas.create_text(275, 325, text="Hits")
canvas.create_text(350, 325, text="Errors")
canvas.create_text(425, 325, text="Record")

# Away fields
away_runs = Entry(root, width=2, font=("Arial", 12))
away_runs.place(x=200-12, y=350-8)
away_hits = Entry(root, width=2, font=("Arial", 12))
away_hits.place(x=275-12, y=350-8)
away_errors = Entry(root, width=2, font=("Arial", 12))
away_errors.place(x=350-12, y=350-8)
away_record = Entry(root, width=5, font=("Arial", 12))
away_record.place(x=425-22, y=350-8)

# home fields
home_runs = Entry(root, width=2, font=("Arial", 12)) # the number of runs the home team has scored, not home runs!
home_runs.place(x=200-12, y=390-8)
home_hits = Entry(root, width=2, font=("Arial", 12))
home_hits.place(x=275-12, y=390-8)
home_errors = Entry(root, width=2, font=("Arial", 12))
home_errors.place(x=350-12, y=390-8)
home_record = Entry(root, width=5, font=("Arial", 12))
home_record.place(x=425-22, y=390-8)


def calculate_clicked():

    # assemble the state array
    state = []
    state.append(float(away_record.get()))
    state.append(float(home_record.get()))
    state.append(float(away_runs.get()))
    state.append(float(home_runs.get()))
    state.append(float(away_hits.get()))
    state.append(float(home_hits.get()))
    state.append(float(away_errors.get()))
    state.append(float(home_errors.get()))
    state.append(float(current_inning.get()))
    state.append(float(bottom_top_inning))
    state.append(float(out_count))
    state.append(float(ball_count))
    state.append(float(strike_count))
    state.append(float(man_on_first))
    state.append(float(man_on_second))
    state.append(float(man_on_third))

    # predict
    x = numpy.array([state])
    y = model.predict(x, verbose=False)[0]

    # write
    canvas.itemconfig(calc_run_line, text=str(round(y[0], 1)))
    canvas.itemconfig(calc_total_line, text=str(round(y[1], 1)))
    canvas.itemconfig(calc_away_ml, text=str(round(y[2], 0)))
    canvas.itemconfig(calc_home_ml, text=str(round(y[3], 0)))
    

# calculate button
calculate_button = Button(root, text="Calculate", width=35, bg="light blue", command=calculate_clicked)
calculate_button.place(x=160, y=425)

# calculations
canvas.create_text(100, 475, text="Run Line")
canvas.create_text(225, 475, text="Total Line")
canvas.create_text(350, 475, text="Away Team ML")
canvas.create_text(475, 475, text="Home Team ML")
calc_run_line = canvas.create_text(100, 475+25, font=("Arial", 14), text="-")
calc_total_line = canvas.create_text(225, 475+25, font=("Arial", 14), text="-")
calc_away_ml = canvas.create_text(350, 475+25, font=("Arial", 14), text="-")
calc_home_ml = canvas.create_text(475, 475+25, font=("Arial", 14), text="-")





def on_click(event):
    widget_clicked_id = event.widget.find_withtag("current")[0]

    global man_on_first
    global man_on_second
    global man_on_third
    global ball_count
    global strike_count
    global out_count
    global bottom_top_inning

    if widget_clicked_id == first_base:
        if man_on_first:
            canvas.itemconfig(widget_clicked_id, fill = "gray")
            man_on_first = False
        else:
            canvas.itemconfig(widget_clicked_id, fill = "yellow")
            man_on_first = True
    elif widget_clicked_id == second_base:
        if man_on_second:
            canvas.itemconfig(widget_clicked_id, fill = "gray")
            man_on_second = False
        else:
            canvas.itemconfig(widget_clicked_id, fill = "yellow")
            man_on_second = True
    elif widget_clicked_id == third_base:
        if man_on_third:
            canvas.itemconfig(widget_clicked_id, fill = "gray")
            man_on_third = False
        else:
            canvas.itemconfig(widget_clicked_id, fill = "yellow")
            man_on_third = True
    elif widget_clicked_id == balls_0 or widget_clicked_id == balls_0_text:
        ball_count = 0
        update_ui_balls()
    elif widget_clicked_id == balls_1 or widget_clicked_id == balls_1_text:
        ball_count = 1
        update_ui_balls()
    elif widget_clicked_id == balls_2 or widget_clicked_id == balls_2_text:
        ball_count = 2
        update_ui_balls()
    elif widget_clicked_id == balls_3 or widget_clicked_id == balls_3_text:
        ball_count = 3
        update_ui_balls()
    elif widget_clicked_id == strikes_0 or widget_clicked_id == strikes_0_text:
        strike_count = 0
        update_ui_strikes()
    elif widget_clicked_id == strikes_1 or widget_clicked_id == strikes_1_text:
        strike_count = 1
        update_ui_strikes()
    elif widget_clicked_id == strikes_2 or widget_clicked_id == strikes_2_text:
        strike_count = 2
        update_ui_strikes()
    elif widget_clicked_id == outs_0 or widget_clicked_id == outs_0_text:
        out_count = 0
        update_ui_outs()
    elif widget_clicked_id == outs_1 or widget_clicked_id == outs_1_text:
        out_count = 1
        update_ui_outs()
    elif widget_clicked_id == outs_2 or widget_clicked_id == outs_2_text:
        out_count = 2
        update_ui_outs()
    elif widget_clicked_id == inning_top or widget_clicked_id == inning_top_text:
        bottom_top_inning = False
        update_ui_bottom_top_inning()
    elif widget_clicked_id == inning_bottom or widget_clicked_id == inning_bottom_text:
        bottom_top_inning = True
        update_ui_bottom_top_inning()
        




def update_ui_balls():
    global balls_0
    global balls_1
    global balls_2
    global balls_3
    global ball_count
    global canvas

    if ball_count == 0:
        canvas.itemconfig(balls_0, fill="yellow")
        canvas.itemconfig(balls_1, fill="light blue")
        canvas.itemconfig(balls_2, fill="light blue")
        canvas.itemconfig(balls_3, fill="light blue")
    elif ball_count == 1:
        canvas.itemconfig(balls_0, fill="light blue")
        canvas.itemconfig(balls_1, fill="yellow")
        canvas.itemconfig(balls_2, fill="light blue")
        canvas.itemconfig(balls_3, fill="light blue")
    elif ball_count == 2:
        canvas.itemconfig(balls_0, fill="light blue")
        canvas.itemconfig(balls_1, fill="light blue")
        canvas.itemconfig(balls_2, fill="yellow")
        canvas.itemconfig(balls_3, fill="light blue")
    elif ball_count == 3:
        canvas.itemconfig(balls_0, fill="light blue")
        canvas.itemconfig(balls_1, fill="light blue")
        canvas.itemconfig(balls_2, fill="light blue")
        canvas.itemconfig(balls_3, fill="yellow")

def update_ui_strikes():
    global strike_count
    global strikes_0
    global strikes_1
    global strikes_2
    global canvas

    if strike_count == 0:
        canvas.itemconfig(strikes_0, fill="yellow")
        canvas.itemconfig(strikes_1, fill="pink")
        canvas.itemconfig(strikes_2, fill="pink")
    elif strike_count == 1:
        canvas.itemconfig(strikes_0, fill="pink")
        canvas.itemconfig(strikes_1, fill="yellow")
        canvas.itemconfig(strikes_2, fill="pink")
    elif strike_count == 2:
        canvas.itemconfig(strikes_0, fill="pink")
        canvas.itemconfig(strikes_1, fill="pink")
        canvas.itemconfig(strikes_2, fill="yellow")
        
def update_ui_outs():
    global out_count
    global outs_0
    global outs_1
    global outs_2
    global canvas

    if out_count == 0:
        canvas.itemconfig(outs_0, fill="yellow")
        canvas.itemconfig(outs_1, fill="orange")
        canvas.itemconfig(outs_2, fill="orange")
    elif out_count == 1:
        canvas.itemconfig(outs_0, fill="orange")
        canvas.itemconfig(outs_1, fill="yellow")
        canvas.itemconfig(outs_2, fill="orange")
    if out_count == 2:
        canvas.itemconfig(outs_0, fill="orange")
        canvas.itemconfig(outs_1, fill="orange")
        canvas.itemconfig(outs_2, fill="yellow")

def update_ui_bottom_top_inning():
    global inning_top
    global inning_bottom
    global canvas
    global bottom_top_inning

    if bottom_top_inning == False:
        canvas.itemconfig(inning_top, fill = "yellow")
        canvas.itemconfig(inning_bottom, fill = "light gray")
    elif bottom_top_inning == True:
        canvas.itemconfig(inning_top, fill = "light gray")
        canvas.itemconfig(inning_bottom, fill = "yellow")






canvas.tag_bind(first_base, "<Button-1>", on_click)
canvas.tag_bind(second_base, "<Button-1>", on_click)
canvas.tag_bind(third_base, "<Button-1>", on_click)
canvas.tag_bind(balls_0, "<Button-1>", on_click)
canvas.tag_bind(balls_1, "<Button-1>", on_click)
canvas.tag_bind(balls_2, "<Button-1>", on_click)
canvas.tag_bind(balls_3, "<Button-1>", on_click)
canvas.tag_bind(balls_0_text, "<Button-1>", on_click)
canvas.tag_bind(balls_1_text, "<Button-1>", on_click)
canvas.tag_bind(balls_2_text, "<Button-1>", on_click)
canvas.tag_bind(balls_3_text, "<Button-1>", on_click)
canvas.tag_bind(strikes_0, "<Button-1>", on_click)
canvas.tag_bind(strikes_1, "<Button-1>", on_click)
canvas.tag_bind(strikes_2, "<Button-1>", on_click)
canvas.tag_bind(strikes_0_text, "<Button-1>", on_click)
canvas.tag_bind(strikes_1_text, "<Button-1>", on_click)
canvas.tag_bind(strikes_2_text, "<Button-1>", on_click)
canvas.tag_bind(outs_0, "<Button-1>", on_click)
canvas.tag_bind(outs_1, "<Button-1>", on_click)
canvas.tag_bind(outs_2, "<Button-1>", on_click)
canvas.tag_bind(outs_0_text, "<Button-1>", on_click)
canvas.tag_bind(outs_1_text, "<Button-1>", on_click)
canvas.tag_bind(outs_2_text, "<Button-1>", on_click)
canvas.tag_bind(inning_bottom, "<Button-1>", on_click)
canvas.tag_bind(inning_top, "<Button-1>", on_click)
canvas.tag_bind(inning_bottom_text, "<Button-1>", on_click)
canvas.tag_bind(inning_top_text, "<Button-1>", on_click)

root.mainloop()