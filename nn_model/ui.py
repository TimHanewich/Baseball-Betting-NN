from tkinter import *


# game state
man_on_first:bool = False
man_on_second:bool = False
man_on_third:bool = False






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
canvas.create_text(200 + 12.5, 188 + 12.5, text="0")
balls_1 = canvas.create_rectangle(225, 188, 250, 188 + 25, fill="light blue")
canvas.create_text(225 + 12.5, 188 + 12.5, text="1")
balls_2 = canvas.create_rectangle(250, 188, 275, 188 + 25, fill="light blue")
canvas.create_text(250 + 12.5, 188 + 12.5, text="2")
balls_3 = canvas.create_rectangle(275, 188, 300, 188 + 25, fill="light blue")
canvas.create_text(275 + 12.5, 188 + 12.5, text="3")

# create strikes
strikes_0 = canvas.create_rectangle(200, 226, 200 + 25, 226 + 25, fill="pink")
canvas.create_text(200 + 12.5, 226 + 12.5, text="0")
strikes_1 = canvas.create_rectangle(225, 226, 225 + 25, 226 + 25, fill="pink")
canvas.create_text(225 + 12.5, 226 + 12.5, text="1")
strikes_2 = canvas.create_rectangle(250, 226, 250 + 25, 226 + 25, fill="pink")
canvas.create_text(250 + 12.5, 226 + 12.5, text="2")

# create outs
outs_0 = canvas.create_rectangle(200, 267, 200 + 25, 267 + 25, fill="orange")
canvas.create_text(200 + 12.5, 267 + 12.5, text="0")
outs_1 = canvas.create_rectangle(225, 267, 225 + 25, 267 + 25, fill="orange")
canvas.create_text(225 + 12.5, 267 + 12.5, text="1")
outs_2 = canvas.create_rectangle(250, 267, 250 + 25, 267 + 25, fill="orange")
canvas.create_text(250 + 12.5, 267 + 12.5, text="2")

# current inning
canvas.create_text(350, 200, text="Inning")
current_inning = Entry(root)
current_inning.place(x=375, y=191)

# top or bottom (batting team)
inning_top = canvas.create_rectangle(350, 225, 420, 250, fill="light gray")
canvas.create_text(350 + 35, 225 + 12.5, text="Top")
inning_bottom = canvas.create_rectangle(420, 225, 490, 250, fill="light gray")
canvas.create_text(420 + 35, 225 + 12.5, text="Bottom")


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




def on_click(event):
    widget_clicked_id = event.widget.find_withtag("current")[0]

    global man_on_first
    global man_on_second
    global man_on_third

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


canvas.tag_bind(first_base, "<Button-1>", on_click)
canvas.tag_bind(second_base, "<Button-1>", on_click)
canvas.tag_bind(third_base, "<Button-1>", on_click)

root.mainloop()