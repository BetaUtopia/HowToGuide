document.addEventListener('DOMContentLoaded', function () {
    // Replace favicon
    var links = document.getElementsByTagName("link");
    for (var linkIndex = 0; linkIndex < links.length; linkIndex++) {
        var link = links[linkIndex];
        if (link && link.rel === 'icon') {
            link.href = '../docs/favicon-light.ico';
        }
    }

    // Replace SVG with image
    function replaceSvgWithImage() {
        var svgElement = document.querySelector('.swagger-ui .topbar-wrapper .link svg');
        if (svgElement) {
            var imgElement = document.createElement('img');
            imgElement.src = '../docs/favicon-light.png';
            imgElement.alt = 'Logo';
            imgElement.style.height = '32px';
            imgElement.style.width = 'auto';

            var parent = svgElement.parentNode;
            parent.replaceChild(imgElement, svgElement);
        } else {
            setTimeout(replaceSvgWithImage, 100);
        }
    }
    replaceSvgWithImage();
});