window.stopTheGame = () ->
	return 0

window.setPlayerNames = (text) ->
	
	list = text.split("|")

	# Find the elements
	ele = document.getElementById("players")


	for line in list
		li = document.createElement("li")
		li.innerHTML = line
		ele.appendChild(li)
	return null

window.populateSessionPages = (sessionInfo) ->
	return null