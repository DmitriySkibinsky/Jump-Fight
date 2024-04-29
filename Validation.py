def find_line_with_content(file_path, content):
    with open(file_path, 'r') as file:
        for line in file:
            if content in line:
                return line.strip()
    return None  # Возвращаем None, если строка не найдена

file_path = './result.txt'
content_to_find = "Test results:"
line_with_content = find_line_with_content(file_path, content_to_find)

if line_with_content is not None:
    words = line_with_content.split()
    if len(words) >= 13:
        print(words[10], words[13])
        if int(words[10]) == 0 and int(words[13]) == 0:
            print("ALL TESTS PASSED")
        else:
            raise ValueError("The elements are not equal to 0.")
    else:
        raise IndexError("The line does not have enough words.")
else:
    raise ValueError("The file does not contain the specified content.")