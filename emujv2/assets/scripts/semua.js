﻿//linkDepan = "https://webapi.ktmb.com.my/ccadapi/api/webapi/";
linkDepan = "http://localhost:58704/api/webapi/";

$(document).ready(function () {
    var x = localStorage.getItem("usrN");
    $("#tempekNamaUsr").html(x + ' <i class="mdi mdi-chevron-down"></i> ');
    var I = localStorage.getItem("usrI");
    var D = localStorage.getItem("depN");
    //var L = localStorage.getItem("locI");
    //var E = localStorage.getItem("eml");
    //var J = localStorage.getItem("jobD");
    $("#profModNama").html(x);
    $("#profModJwtn").html(I);
    //$("#profModEmel").html(E);
    //$("#profModLoc").html(L);
    $("#profModJbtn").html(D);

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
        ayats = 'Something Wrong Happen!';
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
            setTimeout(window.location.replace("CCAD/Auth/Index"), 20000);
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
    localStorage.removeItem("invDP");
    localStorage.removeItem("invDPT");
    localStorage.removeItem("jnsV");
    localStorage.removeItem("valV");
    localStorage.removeItem("invCHT");
    localStorage.removeItem("selHar");
    localStorage.removeItem("selHarE");
    localStorage.removeItem("spotV"); 
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
            setTimeout(window.location.replace("CCAD/Auth/Index"), 20000);
        }
        else if (pnjg > 2) {
            setTimeout(window.location.replace("../CCAD/Auth/Index"), 20000);
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
function senaraiManualChrg() {
    $("#statIdvN").html(localStorage.getItem('usrN'));
    $("#statDepN").html(localStorage.getItem('depN'));
    $.fn.dataTable.ext.errMode = 'throw';
    $.ajax({
        url: linkDepan1 + "/ListCharge",
        type: 'GET',
        dataType: 'json',
        data: {},
        beforeSend: function (request) {
            request.setRequestHeader("Token", localStorage.getItem('main'));
        },
        success: function (data) {
            $("#statIdv").html(data['data1'].length);
            $("#statDep").html(data['depart'].length);

            tebals = ''; rmindv = 0;
            if (data['data1'].length >= 1) {
                $.each(data['data1'], function (index, value) {
                    datesn = "-"; rmindv += parseFloat(value.sender_amount);
                    if (value.sender_dateInvoice != null) {
                        datesn = jenisTarikhJSON(value.sender_dateInvoice.date);
                    }
                    detInvoice = ptongAyat(value.sender_detailInvoice, 100, '...');
                    tebals += '<div class="col-md-3"><div class="card report-card"><h5 class="card-header bg-success text-white mt-0"><i class="fas fa-calendar-check mr-2 font-16"></i>' +
                        datesn + '</h5><div class="card-body"><div class="float-right"><i class="fas fa-lock text-success report-main-icon"></i></div><h4 class="title-text mt-0">' +
                        value.receive_department + '</h4><h3 class="my-3" >RM ' +
                        value.sender_amount + '</h3><h4 class=""><i class="fas fa-info-circle mr-2 text-info font-14"></i>' +
                        detInvoice + '</h4><p><span class="btn btn-secondary btn-circle">' + value.jmlhserv + '</span> Service </p>' +
                        //'<p class="mb-0 text-muted text-truncate">Services</p><ul class="list-unstyled mb-0 text-muted"><li class=""><span></span>serv1</li></ul>'+
                        '</div><button type="button" data-id="' + value.id + '" data-toggle="modal" data-animation="bounce" data-target=".modRekodManCharge" class="btn btn-primary float-right">Details</button></div></div>';
                });
                $("#tempekGrid").html(tebals);

                var counter = 1;
                $('#tableList').DataTable({
                    dom: 'Bfrtip',
                    buttons: [
                        'copy', 'csv', 'excel', 'pdf', 'print'
                    ],
                    responsive: true,
                    data: data['data1'],
                    columns: [
                        { data: "id" },
                        { data: "sender_department" },
                        { data: "sender_costcentre" },
                        { data: "receive_department" },
                        { data: "receive_costcentre" },
                        { data: "sender_amount" },
                        { data: "sender_detailInvoice" },
                        { data: "id" }
                    ],
                    columnDefs: [
                        {
                            targets: -8,
                            render: function (data, type, full, meta) {
                                return counter++;
                            },
                        },
                        {
                            targets: -2,
                            render: function (data, type, full, meta) {
                                return ptongAyat(data, 50, '...');
                            },
                        },
                        {
                            targets: -1,
                            orderable: false,
                            render: function (data, type, full, meta) {
                                return '<button class="btn btn-info" data-id="' + data + '" data-toggle="modal" data-animation="bounce" data-target=".modRekodManCharge" ><i class="fa fa-info"></i> </button>';
                            },
                        }],
                });
            }
            else {
                tebals += '<div class="alert alert-outline-warning alert-warning-shadow mb-0 alert-dismissible fade show" role="alert"><button type = "button" class="close" data - dismiss="alert" aria - label="Close" ><span aria-hidden="true"><i class="mdi mdi-close"></i></span></button ><strong>Sorry!</strong> No Charge Individual.</div > ';
                $("#tempekGrid").html(tebals);
            }

            tebalDs = ''; rmdepart = 0;
            if (data['depart'].length >= 1) {
                $.each(data['depart'], function (index, value) {
                    datesn = "-"; rmdepart += parseFloat(value.sender_amount);
                    if (value.sender_dateInvoice != null) {
                        datesn = jenisTarikhJSON(value.sender_dateInvoice.date);
                    }
                    detInvoice = ptongAyat(value.sender_detailInvoice, 100, '...');
                    tebalDs += '<div class="col-md-3"><div class="card report-card"><h5 class="card-header bg-info text-white mt-0"><i class="fas fa-calendar-check mr-2 font-16"></i>' +
                        datesn + '</h5><div class="card-body"><div class="float-right"><i class="fas fa-unlock-alt text-primary report-main-icon"></i></div>' +
                        '<h4 class="title-text mt-0">' + value.receive_department + '</h4>' + '<h3 class="my-3">RM ' +
                        value.sender_amount + '</h3><h4><i class="fas fa-info-circle mr-2 text-info font-14"></i>' +
                        value.sender_detailInvoice + '</h4><p><span class="btn btn-secondary btn-circle">' + value.jmlhserv + '</span> Service </p>' +
                        //'<p class="mb-0 text-muted text-truncate">Services</p><ul class="list-unstyled mb-0 text-muted"><li class=""><span></span>serv1</li></ul>' +
                        '</div></div></div> ';
                });
                $("#tempekGridDepart").html(tebalDs);

                var counter = 1;
                $('#tableListDepart').DataTable({
                    dom: 'Bfrtip',
                    buttons: [
                        'copy', 'csv', 'excel', 'pdf', 'print'
                    ],
                    responsive: true,
                    data: data['depart'],
                    columns: [
                        { data: "id" },
                        { data: "sender_department" },
                        { data: "sender_costcentre" },
                        { data: "receive_department" },
                        { data: "receive_costcentre" },
                        { data: "sender_amount" },
                        { data: "sender_detailInvoice" },
                        { data: "id" }
                    ],
                    columnDefs: [
                        {
                            targets: -8,
                            render: function (data, type, full, meta) {
                                return counter++;
                            },
                        },
                        {
                            targets: -2,
                            render: function (data, type, full, meta) {
                                return ptongAyat(data, 50, '...');
                            },
                        },
                        {
                            targets: -1,
                            orderable: false,
                            render: function (data, type, full, meta) {
                                return '';//'<button class="btn btn-info" data-id="' + data + '" data-toggle="modal" data-animation="bounce" data-target=".modRekodManCharge" ><i class="fa fa-info"></i> </button>';
                            },
                        }],
                });
            }
            else {
                tebalDs += '<div class="alert alert-outline-warning alert-warning-shadow mb-0 alert-dismissible fade show" role="alert"><button type = "button" class="close" data - dismiss="alert" aria - label="Close" ><span aria-hidden="true"><i class="mdi mdi-close"></i></span></button ><strong>Sorry!</strong> No Charge Department.</div > ';
                $("#tempekGridDepart").html(tebalDs);
            }
            $("#statIdvRM").html('RM ' + rmindv);
            $("#statDepRM").html('RM ' + rmdepart);
        },
        error: function (xhr) {
            swarning();
            window.setTimeout(senaraiManualChrg, 10000);
        },
        complete: function () {
        }
    });
}
function listDepart(pages) {
    dept = localStorage.getItem("invDP");
    $('#lsDepart').empty().append('<option value="-0">Please Select</option>');
    auxArr = [];
    if (pages == 'reportManual') {
        $('#lsDepart').append("<option value='A'>Select All</option>");
        localStorage.setItem("invDPT", "Select All");
    }
    $.ajax({
        url: linkDepan + "GetDepartmentList",
        type: 'GET',
        dataType: 'json',
        data: {},
        beforeSend: function (request) {
            request.setRequestHeader("Token", localStorage.getItem('main'));
        },
        success: function (data) {
            $.each(data, function (index, value) {
                auxArr[index] = "<option value='" + value.gdc_id + "'>" + value.DeptName + "</option>";                
                if (dept == value.gdc_id) {
                    dept = value.gdc_id;
                    localStorage.setItem("invDPT", value.DeptName);
                }
            });
            $('#lsDepart').append(auxArr.join(''));
            $('#lsDepart option[value="' + dept + '"]').attr('selected', 'selected');
        },
        error: function (xhr) {
            swarning();
        },
        complete: function () {
        }
    });
}
function listCharge() {
    idxs = localStorage.getItem("valV");
    $('#lsCharge').empty().append('<option value="-0">Please Select</option>');
    $.ajax({
        url: linkDepan + "GetChargeListGroup",
        type: 'GET',
        dataType: 'json',
        data: {},
        beforeSend: function (request) {
            request.setRequestHeader("Token", localStorage.getItem('main'));
        },
        success: function (data) {
            auxArr = [];
            $.each(data, function (index, value) {
                auxArr[index] = "<option value='" + value.CD_REF_GROUP + "'>" + value.CD_NAME + "</option>";
                if (idxs == value.CD_REF_GROUP) {
                    localStorage.setItem("invCHT", value.CD_NAME);
                }
            });
            $('#lsCharge').append(auxArr.join(''));
            $('#lsCharge option[value="' + idxs + '"]').attr('selected', 'selected');
        },
        error: function (xhr) {
            swarning();
        },
        complete: function () {
        }
    });
}