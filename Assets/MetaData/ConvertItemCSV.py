import csv
with open("item.csv", "r") as itemFile, open("itemConversion.txt", "w") as convertFile:
	csvReader = csv.reader(itemFile, delimiter=',')
	lineCount = 0
	attrDict = []
	convertFile.write("###JohnItemList\n  - ")
	for row in csvReader:
		print(row);
		if (lineCount != 0) and (lineCount != 1):
			convertFile.write("    ")
		for attrId in range(len(row)):
			if lineCount == 0:
				attrDict.append(row[attrId])
			else:
				print(attrId, attrDict[attrId], row[attrId]);
				if attrId == 0:
					continue
				if attrId == 1:
					convertFile.write("item:{");
				convertFile.write(attrDict[attrId] +":"+row[attrId])
				if attrId == (len(row)-1):
					convertFile.write("},\n")
				else:
					convertFile.write(",");
		lineCount+=1;
	convertFile.write("    goto:Continue");


