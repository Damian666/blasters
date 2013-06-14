window.stopTheGame = () ->
	return 0


window.setNews = (text) ->
	newsElement = document.getElementById("newstext")
	newsElement.innerHTML = "<marquee> " + text + "</marquee>"

window.setRoomNames = (text) ->

	list = text.split("|")
	list.reverse()

	# Find the elements
	elements = document.getElementsByClassName("roomtext")

	for element in elements
		element.innerHTML = ""

		if list.length == 0
			continue

		element.innerHTML = list.pop()	


	return null	

window.setPlayerNames = (text) ->
	
	list = text.split("|")

	# Find the elements
	ele = document.getElementById("players")
	ele.innerHTML = ""

	for line in list
		li = document.createElement("li")
		li.innerHTML = line
		ele.appendChild(li)
	return null

window.populateSessionPages = (sessionInfo) ->
	return null