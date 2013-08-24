class ViewManager
	
	constructor: ->
		# Do something
	
	setView: (viewName) ->
		$("#contentwrapper").empty()
		@currentViewName = viewName
		$.get "views/#{viewName}/template.html", (data) ->
			console.log(data)
			$("#contentwrapper").html data
		,'text'			