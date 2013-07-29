function dload() {
    console.log('\n\n\n\ntest\n\n\n\n');
    var img = document.getElementById('screenCanvas');
    var img2 = new Image();
    img2.onload = function () {
        Pixastic.process(img, "overlay", {
            amount: 1,
            mode: "overlay",
            image: img2
        });
    }
    img2.src = 'images/login-panel.png';
}