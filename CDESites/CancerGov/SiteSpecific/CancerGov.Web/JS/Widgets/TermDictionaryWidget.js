iframe = document.createElement('IFRAME');
iframe.style.width = '260px';
iframe.style.height = '340px';
iframe.frameBorder= '0';


iframe.src = "http://localhost:7001/JS/Widgets/TermDictionaryWidget.htm";


iframe.id = 'container';
document.body.appendChild(iframe);

