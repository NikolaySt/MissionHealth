/* Write here your custom javascript codes */

function initSharesCount() {
    $URL = $(location).attr('href');
    // Facebook Shares Count
    $.getJSON('http://graph.facebook.com/?id=' + $URL, function (fbdata) {
        var shares = $(".post-shares").find("shares");
        var value = shares.text()
        shares.text(value + ReplaceNumberWithCommas(fbdata.shares))
    });

    // Twitter Shares Count
    $.getJSON('http://cdn.api.twitter.com/1/urls/count.json?url=' + $URL + '&callback=?', function (twitdata) {
        var shares = $(".post-shares").find("shares");
        var value = shares.text()
        shares.text(value + ReplaceNumberWithCommas(twitdata.count))
    });

    // LinkIn Shares Count
    $.getJSON('http://www.linkedin.com/countserv/count/share?url=' + $URL + '&callback=?', function (linkdindata) {
        var shares = $(".post-shares").find("shares");
        var value = shares.text()
        shares.text(value + ReplaceNumberWithCommas(twitdata.count))
    });

    $('#data-tab').fadeIn();
};

// Format Number functions
function ReplaceNumberWithCommas(yourNumber) {
    //Seperates the components of the number
    var components = yourNumber.toString().split(".");
    //Comma-fies the first part
    components[0] = components[0].replace(/\B(?=(\d{3})+(?!\d))/g, "");
    //Combines the two sections
    return components.join("");
}

function initDisqusComments() {
    $.ajax({
        type: 'GET',
        url: "https://disqus.com/api/3.0/threads/set.jsonp",
        data: { api_key: disqusPublicKey, forum: "bezgram", thread: urlArray }, // URL method
        cache: false,
        dataType: 'jsonp',
        success: function (result) {

            for (var i in result.response) {

                var countText = " comments";
                var count = result.response[i].posts;

                if (count == 1)
                    countText = " comment";

                $('div[data-disqus-url="' + result.response[i].link + '"]').html('<h4>' + count + countText + '</h4>');

            }
        }
    });
}

(function ($) {
    var _loadingArticle = false;
    var _loadingArticleEnd = false;
    $.fn.loadingArticles = function (urlAction, container) {
        if (_loadingArticle || _loadingArticleEnd) return;
        var loader = $('.loading');
        var hT = loader.offset().top;
        var hH = loader.outerHeight();
        var wH = $(window).height();
        var wS = $(this).scrollTop();
        if (wS > (hT + hH - wH)) {
            var page = loader.data("page");
            loader.html(spinnerCircle());
            _loadingArticle = true;
            if (urlAction.indexOf("?") >= 0) {
                urlAction = urlAction + "&page=" + page;
            }else{
                urlAction = urlAction + "?page=" + page;
            }
            $.ajax({
                url: urlAction,
            }).done(function (html) {
                loader.html("");
                container.append(html);
                page++;
                loader.data("page", page);
                _loadingArticleEnd = html == "";
                _loadingArticle = false;
            }).fail(function (jqXHR, textStatus) {
                loader.html("");
                _loadingArticleEnd = true;
                _loadingArticle = false;
            });
        };
        return this;
    };
})(jQuery);




