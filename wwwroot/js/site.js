// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Check if the user has provided consent
var hasConsent = localStorage.getItem('cookieConsent');
if (!hasConsent) {
    // Display the cookie consent notification
    showCookieConsent();
}

function showCookieConsent() {
    var consentDiv = document.createElement('div');
    consentDiv.id = 'cookieConsent';
    consentDiv.innerHTML = `
                    <div class="cookie-notification">
                        This website uses cookies to ensure you get the best experience on our website.
                        <button onclick="acceptCookies()">Accept</button>
                    </div>`;

    // Get a reference to the first child node of the body
    var firstChild = document.body.firstChild;


    // Insert the consentDiv before the firstChild
    document.body.insertBefore(consentDiv, firstChild);
}

function acceptCookies() {
    // Set the consent flag in localStorage
    localStorage.setItem('cookieConsent', 'true');
    // Hide the cookie consent notification
    document.getElementById('cookieConsent').style.display = 'none';
}

(function ($) {

    "use strict";

    var searchPopup = function () {
        // open search box
        $('#header-nav').on('click', '.search-button', function (e) {
            $('.search-popup').toggleClass('is-visible');
        });

        $('#header-nav').on('click', '.btn-close-search', function (e) {
            $('.search-popup').toggleClass('is-visible');
        });

        $(".search-popup-trigger").on("click", function (b) {
            b.preventDefault();
            $(".search-popup").addClass("is-visible"),
                setTimeout(function () {
                    $(".search-popup").find("#search-popup").focus()
                }, 350)
        }),
            $(".search-popup").on("click", function (b) {
                ($(b.target).is(".search-popup-close") || $(b.target).is(".search-popup-close svg") || $(b.target).is(".search-popup-close path") || $(b.target).is(".search-popup")) && (b.preventDefault(),
                    $(this).removeClass("is-visible"))
            }),
            $(document).keyup(function (b) {
                "27" === b.which && $(".search-popup").removeClass("is-visible")
            })
    }

    var initProductQty = function () {

        $('.product-qty').each(function () {

            var $el_product = $(this);
            var quantity = 0;

            $el_product.find('.quantity-right-plus').click(function (e) {
                e.preventDefault();
                var quantity = parseInt($el_product.find('#quantity').val());
                $el_product.find('#quantity').val(quantity + 1);
            });

            $el_product.find('.quantity-left-minus').click(function (e) {
                e.preventDefault();
                var quantity = parseInt($el_product.find('#quantity').val());
                if (quantity > 0) {
                    $el_product.find('#quantity').val(quantity - 1);
                }
            });

        });

    }

    $(document).ready(function () {

        searchPopup();
        initProductQty();

        var swiper = new Swiper(".main-swiper", {
            speed: 500,
            navigation: {
                nextEl: ".swiper-arrow-prev",
                prevEl: ".swiper-arrow-next",
            },
        });

        var swiper = new Swiper(".product-swiper", {
            slidesPerView: 4,
            spaceBetween: 10,
            pagination: {
                el: "#mobile-products .swiper-pagination",
                clickable: true,
            },
            breakpoints: {
                0: {
                    slidesPerView: 2,
                    spaceBetween: 20,
                },
                980: {
                    slidesPerView: 4,
                    spaceBetween: 20,
                }
            },
        });

        var swiper = new Swiper(".product-watch-swiper", {
            slidesPerView: 4,
            spaceBetween: 10,
            pagination: {
                el: "#smart-watches .swiper-pagination",
                clickable: true,
            },
            breakpoints: {
                0: {
                    slidesPerView: 2,
                    spaceBetween: 20,
                },
                980: {
                    slidesPerView: 4,
                    spaceBetween: 20,
                }
            },
        });

        var swiper = new Swiper(".testimonial-swiper", {
            loop: true,
            navigation: {
                nextEl: ".swiper-arrow-prev",
                prevEl: ".swiper-arrow-next",
            },
        });

    }); // End of a document ready

})(jQuery);