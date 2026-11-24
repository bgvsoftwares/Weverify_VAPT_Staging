$(".iframeUpdateStatus").colorbox({ iframe: true, width: "60%", height: "60%" });
$(".callbacks").colorbox({
    //onOpen: function () { alert('onOpen: colorbox is about to open'); },
    //onLoad: function () { alert('onLoad: colorbox has started to load the targeted content'); },
    //onComplete: function () { alert('onComplete: colorbox has displayed the loaded content'); },
    //onCleanup: function () { alert('onCleanup: colorbox has begun the close process'); },
    onClosed: function () {
        //alert('onClosed: colorbox has completely closed');
        window.location.reload(false);
    }
});

$(".iframeUpdateVerDoc").colorbox({ iframe: true, width: "60%", height: "60%" });
$(".callbacks").colorbox({
    onClosed: function () {
        window.location.reload(true);
    }
});

$(".iframeEmailStatus").colorbox({ iframe: true, width: "80%", height: "90%" });

$(".iframeActionHistories").colorbox({ iframe: true, width: "80%", height: "65%" });