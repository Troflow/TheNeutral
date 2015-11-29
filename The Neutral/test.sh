#!/bin/bash

IFS=$'\n'

COUNTER=0
for y in $(find . -iname $1); do
	for x in $(cat $y | grep $2); do
		printf "%s was in: %s" "$x $(pwd $y)" 
		let COUNTER=COUNTER+1
	done
done
echo The counter is $COUNTER
