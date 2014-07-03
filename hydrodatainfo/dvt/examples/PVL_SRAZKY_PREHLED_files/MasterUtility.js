function ZvyrazniTlacitkoSVlajkou(tlacitko) {
    var predpona = tlacitko.src.substring(0, tlacitko.src.length - 4);
    var pripona = tlacitko.src.substring(tlacitko.src.length - 4, tlacitko.src.length);
    if (tlacitko.src.indexOf("_f2") == -1)
        tlacitko.src = predpona + "_f2" + pripona;
}

function ZasedniTlacitkoSVlajkou(tlacitko) {
    if (tlacitko.src.indexOf("_f2") >= 0)
        tlacitko.src = tlacitko.src.replace("_f2", "");
}

function ZvyrazniTlacitkoProVolbuVzhledu(tlacitko) {
    if (tlacitko.src.indexOf("_zasedle") >= 0)
        tlacitko.src = tlacitko.src.replace("_zasedle", "");
}

function ZasedniTlacitkoProVolbuVzhledu(tlacitko) {
    var predpona = tlacitko.src.substring(0, tlacitko.src.length - 4);
    var pripona = tlacitko.src.substring(tlacitko.src.length - 4, tlacitko.src.length);
    if (tlacitko.src.indexOf("_zasedle") == -1)
        tlacitko.src = predpona + "_zasedle" + pripona;
}

function ZvolTemaAplikaceNaZakladeRozliseniKlienta() {
    var temata = ["PC", "SmartPhone", "Text"];
    var vyhovujiciTema = "";
    var minSirkaDispleje = [];
    minSirkaDispleje["PC"] = 1280;
    minSirkaDispleje["SmartPhone"] = 800;
    minSirkaDispleje["Text"] = 0;
    var maxSirkaDispleje = [];
    maxSirkaDispleje["PC"] = Infinity;
    maxSirkaDispleje["SmartPhone"] = 1279;
    maxSirkaDispleje["Text"] = 799;
    var url = window.location.href;
    var sirkaDispleje = screen.width;
    var temaObsazeneVURL = "";
    for (var i = 0; i < temata.length; i++) {
        if (sirkaDispleje >= minSirkaDispleje[temata[i]] && sirkaDispleje <= maxSirkaDispleje[temata[i]]) {
            vyhovujiciTema = temata[i];
        }
        if (url.indexOf("/" + temata[i] + "/") > 0) {
            temaObsazeneVURL = temata[i];
        }
    }
    if (temaObsazeneVURL == "" && vyhovujiciTema != "") {
        var jazyky = ["cz", "sk", "en", "de", "hu", "pl"];
        var jazykObsazenyVURL = "";
        for (var i = 0; i < jazyky.length; i++) {
            if (url.indexOf("/" + jazyky[i] + "/") > 0) {
                jazykObsazenyVURL = jazyky[i];
                break;
            }
        }
        var novaURL = ""
        if (jazykObsazenyVURL != "") {
            var zacatek = url.substring(0, url.indexOf("/" + jazykObsazenyVURL + "/") + 4);
            var konec = url.substring(url.indexOf("/" + jazykObsazenyVURL + "/") + 4);
            novaURL = zacatek + vyhovujiciTema + "/" + konec;
            window.location.href = novaURL;
            return false;
        }
        else {
            var indexVlozeniTematu = -1;
            if (url.indexOf(".aspx/") > 0) {
                var pomUrl = url.substring(0, url.indexOf(".aspx/"));
                indexVlozeniTematu = pomUrl.lastIndexOf("/") + 1;
            }
            else
                indexVlozeniTematu = url.lastIndexOf("/") + 1;
            if (indexVlozeniTematu > 0) {
                var zacatek = url.substring(0, indexVlozeniTematu);
                var konec = url.substring(indexVlozeniTematu);
                novaURL = zacatek + vyhovujiciTema + "/" + konec;
                window.location.href = novaURL;
                return false;
            }
        }
    }
}