def get_last_line(file_path):
    with open(file_path, 'r') as file:
        lines = file.readlines()
        if lines:
            last_line = lines[-1].strip()
            return last_line
        else:
            return None  # Возвращаем None, если файл пуст

file_path = './result.txt'  
last_line = get_last_line(file_path)

if last_line is not None:
    last_line = last_line.split()
    if len(last_line) >= 13:
        print(last_line[10], last_line[13])
        if int(last_line[10]) == 0 and int(last_line[13]) == 0:
            print("ALL TESTS PASSED")
        else:
            raise ValueError("The elements are not equal to 0.")
    else:
        raise IndexError("The line does not have enough words.")
else:
    raise ValueError("The file is empty.")