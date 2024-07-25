from email.mime import image
import cv2
import os


def main(imagePath: str, tempPath: str) -> None:
    img = cv2.imread(imagePath)
    if img is None:
        raise Exception("Image not found.")
        #return

    dimensions = img.shape
    img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

    #script_dir = os.path.dirname(os.path.abspath(__file__))
    #save_path = os.path.join(script_dir, 'preprocessed_image.jpg')

    _, img = cv2.threshold(img, 130, 255, cv2.THRESH_BINARY)

    cv2.imwrite(tempPath, img)

    return

