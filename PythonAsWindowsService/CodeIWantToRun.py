import datetime
import time

# Creates a log every 10 seconds.
log_message = "Testing Service"

def write_to_log(message):
    log_file_path = "C:\Test\log.txt"

    current_time = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
    log_entry = f"{current_time} - {message}\n"

    with open(log_file_path, "a") as log_file:
        log_file.write(log_entry)

while True:
    write_to_log(log_message)
    time.sleep(10)
