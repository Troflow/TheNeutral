#!/bin/bash

COUNTER=0
for y in $(find . -iname *.cs); do
	echo $y
	D = cat $y;
	echo $d
	for x in $(grep class $d); do
		echo line: $x
		let COUNTER=COUNTER+1
	done
done
echo The counter is $COUNTER

