(function ($) {

    const languageMapping = {
        'English': 'en',
        '简体中文': 'zh-Hans',
        '繁體中文': 'zh-Hant'
    }

    $('#SettingsForm').change(function () {
        $('#btnSave').removeAttr('disabled')
    })

    $('#SettingsForm').submit(function (e) {
        e.preventDefault();

        var selectedLanguageId = $('#Language').val();
        var userId = $('#userId').val();

        Swal.fire({
            title: 'Loading...', allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading()
            }
        })

        volo.abp.identity.identityUser.get(userId).done(function (profile) {
            profile.extraProperties.LanguageId = selectedLanguageId;

            volo.abp.identity.identityUser.update(userId, profile).done(function () {

                Swal.clickCancel();
    
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'bottom-end',
                    showConfirmButton: false,
                    timer: 3000,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'success',
                    title: 'language updated successfully'
                }).then(function () {
                    var lang = languageMapping[$('#Language option:selected').text().trim()]
                    location.href = '/Abp/Languages/Switch?culture=' + lang + '&uiCulture=' + lang + '&returnUrl=%2FAccount%2FManage';
                })
            })
        })

    })
    
})(jQuery);
