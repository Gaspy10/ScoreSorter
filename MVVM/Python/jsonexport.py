import pytesseract
import PIL.Image
#from lists import names
import cv2
import json


class FinalMembers:
    def __init__(self, name, points):
        self.name = name
        self.points = points

    def to_dict(self):
        return {
            'name': self.name,
            'points': self.points
        }


def main(imagePath: str, tempPath: str) ->None:
    myconfig = r"-l ces"
    text = pytesseract.image_to_string(PIL.Image.open(imagePath), config=myconfig)
    # print(text)

    lastSuccessful = ""
    previousIndex = -1
    slices = []

    for i in range(0, len(text)):
        if (text[i] == "\n"):
            slice = text[(previousIndex + 1):i]
            sliceLength = len(slice)
            if (len(slice) > 65):
                subslice1 = slice[0:(sliceLength / 2)]
                subslice2 = slice[(sliceLength / 2):sliceLength]
                slices.append(subslice1)
                slices.append(subslice2)
            elif (len(slice) > 10):
                slices.append(slice)
            previousIndex = i

    """for items in names:
        print(f"Working on '{items}':")
    
        try:
            index = text.index(items)
            lastSuccessful = items
            print("Job successful.")
    
        except ValueError:
            print("Could not find.")
            continue
    
        slice = text[previousIndex: index]
        slices.append(slice)
    
        previousIndex = index
    
    del slices[0]
    
    lastIndex = text.index(lastSuccessful)
    slices.append(text[lastIndex:(len(text)-1)])"""

    # print(slices)
    isInt = False
    numberInts = 0
    points = ""
    pointsList = []

    for items in slices:
        for i in range(0, len(items)):
            try:
                result = int(items[i])
                if (isInt == False):
                    numberInts += 1
                if (numberInts == 3):
                    points += items[i]
                isInt = True

            except ValueError as e:
                if (items[i] == "," or items[i] == "."):
                    if (numberInts == 3):
                        points += items[i]
                else:
                    isInt = False

            if (i == (len(items) - 1)):
                if (numberInts != 3):
                    points = "-1"

                pointsList.append(points)
                points = ""
                isInt = False
                numberInts = 0

    """i = 0
    for items in names:
        try:
            print(f"{i+1}. {items}: {pointsList[i]}")
            i += 1
        except IndexError:
            print()"""
    actualSlices = []

    for items in slices:
        for i in range(0, len(items)):
            try:
                result = int(items[i])
                actualSlice = items[0:i]
                actualSlices.append(actualSlice)
                break
            except ValueError:
                if (i == (len(items) - 1)):
                    actualSlices.append(items)

    unicode_code_points = [283, 233, 353, 345, 253, 225, 237, 233, 253, 32, 269, 328, 367]
    cleanedActualSlices = []

    for items in actualSlices:
        deletableIndexes = []
        for i in range(0, len(items)):
            ascii_value = ord(items[i])
            deleteChar = True

            if (not ((ascii_value >= 65 and ascii_value <= 90) or (ascii_value >= 97 and ascii_value <= 122))):
                for points in unicode_code_points:
                    if (points == ord(items[i])):
                        deleteChar = False
                        break
            else:
                deleteChar = False

            if (deleteChar):
                deletableIndexes.append(i)

        append = items
        # cursor += 1
        for index in reversed(deletableIndexes):
            append = items[:index] + items[index + 1:]

        cleanedActualSlices.append(append)

    if (len(actualSlices) != len(pointsList)):
        print("An error occured")
        exit()

    FinalList = []
    for i in range(0, len(actualSlices)):
        # print(f"{i+1}. {actualSlices[i]}: {pointsList[i]}")
        personData = FinalMembers(cleanedActualSlices[i], pointsList[i])
        FinalList.append(personData)

    for objectMember in FinalList:
        print(objectMember.name)
        print(objectMember.points + "\n")

    #export json
    data = [member.to_dict() for member in FinalList]
    
    # Export JSON
    with open(tempPath, 'w') as file:
        json.dump(data, file, indent=4)
    
    return

