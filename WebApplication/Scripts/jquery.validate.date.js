$(function () {
    $.validator.methods.date = function (value, element) {
        //if ($.browser.webkit) {
        //    var d = new Date();
        //    return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
        //}
        //else {
        //    return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
        //}

        //CREATE NEW DATE
        //BECAUSE JAVACRIPT DON'T KNOW dd/MM/yyy format, we have to do it manually...
        var d = value.split('/');
        var h = value.split(' ')[1];
        var day = parseInt(d[0]);
        var month = parseInt(d[1]);
        var year = parseInt(d[2]);

        if (h) {
            h = h.split(':');
            var hour = parseInt(h[0]);
            var min = parseInt(h[1]);
            var s = parseInt(h[3]);

            (isNaN(s)) ? s = 0 : null;

            d = new Date(year, month, day, hour, min, s);
        }
        else
        {
            d = new Date(year, month, day);
        }
        
        

        return this.optional(element) || !/Invalid|NaN/.test(d);
    };
});