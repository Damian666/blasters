# This file contains functions

window.setPlayerNames = (text) ->
	
	list = text.split("|")

	# Find the elements
	elements = document.getElementsByClassName("playerbox")

	for element in elements
		playerBox = element.getElementsByClassName("playername")[0]
		playerBox.innerHTML = list.pop();

	return null
