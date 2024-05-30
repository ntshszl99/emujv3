//linkDepan = "https://webapi.ktmb.com.my/emujv2api/api/webapi/";
linkDepan = "http://localhost:16049/api/webapi/";

$(document).ready(function () {
    var x = localStorage.getItem("usrN");
    $("#tempekNamaUsr").html(x);
    $("#profNama").html(x);
    $("#Nama").html(x);

    var UI = localStorage.getItem("usrI");
    $("#tempekUsrId").html(UI);

    var UD = localStorage.getItem("usrD");
    $("#tempeUsrDesig").html(UD);
    $("#UsrDesig").html(UD);

    var yrs = localStorage.getItem("usrY");
    $("#tempekYrs").html(yrs + ' Years');

    var IC = localStorage.getItem("IC");
    $("#tempekIC").html(IC);

    var age = localStorage.getItem("age");
    $("#tempekAge").html(age + ' Years');

    var PN = localStorage.getItem("phoneNum");
    $("#tempekNombor").html(PN);

    var DL = localStorage.getItem("depL");
    $("#tempekDeptLoc").html(DL);

    var DI = localStorage.getItem("depI");
    $("#tempekDeptId").html(DI);

    var DN = localStorage.getItem("depN");
    $("#tempekNamaDept").html(DN);

    var LI = localStorage.getItem("lvlI");
    $("#tempekLvlId").html(LI);

    var UL = localStorage.getItem("usrLvl");
    $("#tempekUsrLvl").html(UL);

    var UR = localStorage.getItem("usrR");
    $("#tempekUsrRegion").html(UR);

    var URE = localStorage.getItem("usrRE");
    $("#tempekUsrRegionE").html(URE);

    var kmu = localStorage.getItem("usrKmu");
    $("#tempekUsrKMUJE").html(kmu);

    var SCN = localStorage.getItem("scn");
    $("#tempekScn").html(SCN);

    var M = localStorage.getItem("kmuj");
    $("#tempekKMUJ").html(M);
   

    $('#nakBlajar').click(function () {
        salertSave('Start Learn.');
        hopscotch.startTour(jalan2());
    });


});





function jenisTarikhJSON(valx) {
    //var arrBuln = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
    var n = valx.search("-");
    if (n >= 0) {
        var ptongAR = valx.split("-");
        var ptong2AR = ptongAR[2].split(" ");
        var hari = ptongAR[0];
        var bulanI = ptongAR[1];
        var tahun = ptong2AR[0];
        return tahun + "." + bulanI + "." + hari;
    }
    else {
        return valx;
    }
}
function jenisMasaJSON(valx) {
    var n = valx.search(":");
    if (n >= 0) {
        var ptongAR1 = valx.split(" "); //alert(Object.values(ptongAR));
        var ptongAR = ptongAR1[1].split("."); //alert(ptongAR[0]);
        return ptongAR[0];
    }
    else {
        return valx;
    }
}

function salertSave(ayats) {
    if (ayats === undefined) {
        ayats = 'Information has been saved';
    }
    var Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        onOpen: function (toast) {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    })

    Toast.fire({
        icon: 'success',
        title: ayats
    })
}
function swarning(ayats) {
    if (ayats === undefined) {
        ayats = 'Something Wrong !';
    }

    Swal.fire({
        title: 'Warning!',
        html: ayats,
        /*text: ayats,*/
        type: 'warning',
        showClass: {
            popup: 'animated fadeInDown faster'
        },
        hideClass: {
            popup: 'animated fadeOutUp faster'
        },
        icon: 'error'
    });
}

function jenisTarikh(valx) {//
    var arrBuln = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
    var n = valx.search("/");
    if (n >= 0) {
        var ptongAR = valx.split("/");
        var ptong2AR = ptongAR[2].split(" ");
        var hari = ptongAR[0]; //console.log(ptongAR);
        if (hari.length <= 1) {
            hari = '0' + hari;
        }
        var bulanI = ptongAR[1];

        /*var hari = ptongAR[1];
        if (hari.length <= 1) {
            hari = '0' + hari;
        }
        var bulanI = ptongAR[0];*/
        var tahun = ptong2AR[0];
        return hari + "-" + arrBuln[bulanI - 1] + "-" + tahun;
    }
    else {
        return valx;
    }
    //return valx;
}
function checkSess() {

    uriR = window.location.pathname;
    ptgs = uriR.split("/");
    pnjg = (ptgs.length);

    const value = getWithExpiry('perang');
    if (!value) {
        localStorage.clear();// alert(pnjg);
        if (pnjg <= 3) {
            setTimeout(window.location.replace("emujv2/Auth/Index"), 20000);
        }
        else if (pnjg == 4) {
            setTimeout(window.location.replace("../Auth/Index"), 20000);
        }
        else if (pnjg == 5) {
            setTimeout(window.location.replace("../../Auth/Index"), 20000);
        }
    }
}

function checkStore() {
    localStorage.removeItem("selHar");
    localStorage.removeItem("selHarE");
    localStorage.removeItem("spotV");
    localStorage.removeItem("selScn");
    localStorage.removeItem("selKmj");
    localStorage.removeItem("selGng");
    localStorage.removeItem("selRgn");
    localStorage.removeItem("stfID");
}
function setWithExpiry(key, value, minutes) {
    var now = new Date();//var minutes = 1;
    masa = now.setTime(now.getTime() + (minutes * 60 * 1000));
    const item = {
        value: value,
        expiry: masa
    }
    localStorage.setItem(key, JSON.stringify(item))
}
function getWithExpiry(key) {
    const itemStr = localStorage.getItem(key);
    if (!itemStr) {
        return null;
    }
    const item = JSON.parse(itemStr);
    const now = new Date();
    if (now.getTime() > item.expiry) {
        localStorage.removeItem(key);
        return null;
    }
    return item.value
}
function checkExt() {
    uriR = window.location.pathname;
    ptgs = uriR.split("/");
    pnjg = ptgs.length;
    if (localStorage.getItem("main") === null) {
        if (pnjg == 2) {
            setTimeout(window.location.replace("emujv2/Auth/Index"), 20000);
        }
        else if (pnjg > 2) {
            setTimeout(window.location.replace("../emujv2/Auth/Index"), 20000);
        }
    }
}
function buangParts(valx) {
    return valx.replace(/([\[\]]+)/g, "\\$1");
}
function bersihInp(valx) {
    return $.trim(valx);
}
function ptongAyat(str, length, ending) {
    if (length == null) {
        length = 100;
    }
    if (ending == null) {
        ending = '...';
    }
    if (str.length > length) {
        return str.substring(0, length - ending.length) + ending;
    } else {
        return str;
    }
};
function formatCurr1(valx) {
    //return (valx).toLocaleString('en', { maximumFractionDigits: 2 });
    return (valx).toLocaleString();
    //return valx;
}
function formatCurr(valx) {
    var parts = valx.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
function getFormData(data) {
    var unindexed_array = data;
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}
function putusAyat(str) {
    return res = str.replace(" ", "<br>");
}
function deltrow(idcol) {
    $("#" + idcol).fadeOut(600, function () {
        $("#" + idcol).remove();
    });
}


