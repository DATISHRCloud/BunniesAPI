var swgrimg = document.getElementsByClassName('logo__img')[0];
swgrimg.setAttribute('src', 'https://content.datis.com/e3/images/datis_poweredbydatis.png');
swgrimg.removeAttribute('height');
swgrimg.setAttribute('width', '140px');

var swgrlnk = document.getElementById('logo');
swgrlnk.removeAttribute('href');

document.getElementsByClassName('logo__title')[0].textContent = '';