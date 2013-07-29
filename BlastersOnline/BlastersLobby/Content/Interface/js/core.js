function init() {
    loadState("login");
}

function loadPage(page) {
    console.log("states/" + page + ".html");
    $("#page-body").load("states/" + page + ".html",
        function (response, status, xhr) {
            if (status == "error") {
                var msg = "Sorry but there was an error: ";
                $("#page-body").html(msg + xhr.status + " " + xhr.statusText);
            }
        });
}

function loadState(state) {
    loadPage(state);
    $.getScript(
        "js/" + state + ".js",
        function onLoad(state) {

        }
    );
}