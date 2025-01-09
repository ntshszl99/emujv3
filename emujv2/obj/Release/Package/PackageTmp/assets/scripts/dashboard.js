$(document).ready(function () {

	$('.datepicker').datepicker();
	uriR = window.location.pathname;
	ptgs = uriR.split("/");
	mukas = ptgs[2];
	subms = ptgs[3];

	$("#BnewCharge").click(function () {
		setTimeout(window.location.replace('NewCharge'), 10000);
	});
	$("#BdashBack").click(function () {
		setTimeout(window.location.replace('ListCharge'), 10000);
	});
	if (!mukas) {
		$("#BnewCharge").click(function () {
			setTimeout(window.location.replace('Home/NewCharge'), 10000);
		});
	}
});

/*function salertSave() {
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
		title: 'Information has been saved'
	})
}
function swarning() {
	Swal.fire({
		title: 'Warning!',
		text: 'Something Wrong Happen!',
		type: 'warning',
		showClass: {
			popup: 'animated fadeInDown faster'
		},
		hideClass: {
			popup: 'animated fadeOutUp faster'
		},
		icon: 'error'
	});
}*/
