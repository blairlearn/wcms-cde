FSR.surveydefs = [{
    name: 'feedback2',
    invite: {
        when: 'onentry'
    },
    pop: {
        when: 'later'
    },
    criteria: {
        sp: 0,
        lf: 0
    },
    include: {
        urls: ['feedback2placeholder']
    }
},{
    name: 'feedback1',
    invite: {
        when: 'onentry'
    },
    pop: {
        when: 'later'
    },
    criteria: {
        sp: 0,
        lf: 0
    },
    include: {
        urls: ['feedback1placeholder']
    }
},{
    name: 'nci-spanish',
    invite: {
        when: 'onentry',
        content: '<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><html><head><meta http-equiv=\"content-type\" content=\"text/html; charset=ISO-8859-1\"></head><body><table id=\"fsrminvite\"><tbody><tr><td class=\"fsrmi_sitelogo\" width=\"49%\"><img src=\"{%baseHref%}sitelogo.gif\" alt=\"Site Logo\"></td><td width=\"2%\">&nbsp;</td><td class=\"fsrmi_fsrlogo\" width=\"49%\"><img src=\"{%baseHref%}fsrlogo.gif\" alt=\"Site Logo\"></td></tr><tr><td><b>Gracias por visitar el sitio web en español del Instituto Nacional del Cáncer.</b></td><td>&nbsp;</td><td><b>Welcome to the National Cancer Institute Web site in Spanish.</b></td></tr><tr><td>¡Necesitamos su ayuda!</td><td>&nbsp;</td><td>We Need Your Help!</td></tr><tr><td>Su opinión es muy importante.</td><td>&nbsp;</td><td>Your feedback is important to us.</td></tr><tr><td>Durante su visita a nuestro sitio web hoy, posiblemente le pediremos que participe en una encuesta sobre su visita. Sus comentarios nos ayudarán a mejorar nuestro sitio. </td><td>&nbsp;</td><td>During your visit to our Web site today, you may be asked to participate in a survey about your visit. Your comments will help us to make the site more useful. </td></tr><tr><td>Por favor, haga clic en Sí para compartir su opinión con nosotros.</td><td>&nbsp;</td><td>Please click on Yes to share your opinions.</td></tr><tr><td style=\"text-align: center;\" nowrap=\"true\"><button onmouseover=\"className=\'fsr_button fsr_mouseover_accept\';\" onmouseout=\"className=\'fsr_button\';\" class=\"fsr_button\" id=\"accept1\" onclick=\"FSR._accepted(\'sp\')\">Sí</button><button onmouseover=\"className=\'fsr_button fsr_mouseover_decline\';\" onmouseout=\"className=\'fsr_button\';\" class=\"fsr_button\" id=\"decline1\" onclick=\"FSR._declined(\'sp\')\">No Gracias</button></td><td>&nbsp;</td><td style=\"text-align: center;\" nowrap=\"true\"><button onmouseover=\"className=\'fsr_button fsr_mouseover_accept\';\" onmouseout=\"className=\'fsr_button\';\" class=\"fsr_button\" id=\"accept2\" onclick=\"FSR._accepted(\'en\')\">Yes</button><button onmouseover=\"className=\'fsr_button fsr_mouseover_decline\';\" onmouseout=\"className=\'fsr_button\';\" class=\"fsr_button\" id=\"decline2\" onclick=\"FSR._declined(\'en\')\">No Thanks</button></td></tr></tbody></table></body></html>',
        width: '625',
        buttons: false,
        footer: '<div div id=\"fsrcontainer\"><div style=\"float:right;font-size:8pt;\"><a target="_blank" title="Validate TRUSTe privacy certification" href="//privacy-policy.truste.com/click-with-confidence/ctv/en/www.foreseeresults.com/seal_m"><img border=\"0\" src=\"{%baseHref%}truste.png\" alt=\"Validate TRUSTe Privacy Certification\"></a></div></div>'
    },
	pin: 1,
    pop: {
        when: 'later',
        after: 'leaving-section'
    },
    criteria: {
        sp: 100,
        lf: 4
    },
    qualifier: {
        content: '<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><HTML><HEAD><TITLE>Foresee Survey</TITLE><meta http-equiv=\"Content-Type\" content= \"text/html; charset=utf-8\"></HEAD><BODY><form name=\"fsrform\" style=\"margin: 0pt;\" onsubmit=\"return false;\"><div id=\"fsrinvite\"><div id=\"fsrcontainer\"><div class=\"fsri_sitelogo\"><img src=\"{%baseHref%}sitelogo.gif\" alt=\"Site Logo\"></div><div class=\"fsri_fsrlogo\"><img src=\"{%baseHref%}fsrlogo.gif\" alt=\"Site Logo\"></div></div><div class=\"fsri_body\">Earlier in your visit, you agreed to share your opinions about our Web site with us. We appreciate the opportunity to hear from you!<br><br>Please click on Continue below to proceed to the survey.<br><br></div><div class=\"fsri_footer\"><button onmouseover=\"className=\'fsr_button fsr_mouseover_accept\';\" onmouseout=\"className=\'fsr_button\';\" class=\"fsr_button\" id=\"continue\" onclick=\'FSR._qualified();\'>Continue</button></div></div></form></BODY></HTML>',
        locales: [{
            locale: 'sp',
            content: '<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><HTML><HEAD><TITLE>Foresee Survey</TITLE><meta http-equiv=\"Content-Type\" content= \"text/html; charset=utf-8\"></HEAD><BODY><form name=\"fsrform\" style=\"margin: 0pt;\" onsubmit=\"return false;\"><div id=\"fsrinvite\"><div id=\"fsrcontainer\"><div class=\"fsri_sitelogo\"><img src=\"{%baseHref%}sitelogo.gif\" alt=\"Site Logo\"></div><div class=\"fsri_fsrlogo\"><img src=\"{%baseHref%}fsrlogo.gif\" alt=\"Site Logo\"></div></div><div class=\"fsri_body\">Hace unos momentos, usted aceptó compartir con nosotros su opinión sobre nuestro sitio web. Le agradecemos la oportunidad que nos da de saber lo que piensa.<br><br>Haga clic, por favor, en Continuar, abajo, para pasar a la encuesta.<br><br></div><div class=\"fsri_footer\"><button onmouseover=\"className=\'fsr_button fsr_mouseover_accept\';\" onmouseout=\"className=\'fsr_button\';\" class=\"fsr_button\" id=\"continue\" onclick=\'FSR._qualified();\'>Continuar</button></div></div></form></BODY></HTML>',
        footer: '<div div id=\"fsrcontainer\"><div style=\"float:left;width:80%;font-size:8pt;text-align:left;line-height:12px;\">Esta encuesta se realiza a través de una empresa independiente, ForeSee, en nombre de la página web que estás visitando.</div><div style=\"float:right;font-size:8pt;\"><a target="_blank" title="Validate TRUSTe privacy certification" href="//privacy-policy.truste.com/click-with-confidence/ctv/en/www.foreseeresults.com/seal_m"><img border=\"0\" src=\"{%baseHref%}truste.png\" alt=\"Validate TRUSTe Privacy Certification\"></a></div></div>'
        }],
        width: '500',
        height: '500',
        buttons: false
    },
    links: {
        cancel: [{
            tag: 'a',
            attribute: 'href',
            patterns: ['xlivehelp']
        }]
    },
    include: {
        urls: ['/espanol', '/diccionario', 'lang=spanish']
    }
}, {
    name: 'nci-main',
    invite: {
        when: 'onentry',
        //url: 'invite.html'
        content: '<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\"><HTML><HEAD><TITLE>Foresee Invite</TITLE><meta http-equiv=\"Content-Type\" content= \"text/html; charset=utf-8\"></HEAD><BODY><div id=\"fsrinvite\"><div id=\"fsrcontainer\"><div class=\"fsri_sitelogo\"><img src=\"{%baseHref%}sitelogo.gif\" alt=\"Site Logo\"></div><div class=\"fsri_fsrlogo\"><img src=\"{%baseHref%}fsrlogo.gif\" alt=\"Site Logo\"></div></div><div class=\"fsri_body\"><b>Welcome to the National Cancer Institute Web site.</b><br><br>We Need Your Help!<br><br>During your visit to our Web site today, you may be asked to participate in a survey about your experience with the site.<br><br>Your comments will help us to make the site more useful. We appreciate the opportunity to hear from you.<br><br>Are you willing to participate in our survey?</div></div></BODY></HTML>'
    },
    pop: {
        when: 'later'
    },
    criteria: {
        sp: 4,
        lf: 4
    },
    links: {
        cancel: [{
            tag: 'a',
            attribute: 'href',
            patterns: ['xlivehelp']
        }]
    },
    include: {
        urls: ['.']
    }
}];
FSR.properties = {
    repeatdays: 60,
    
    repeatoverride: false,
    
    altcookie: {},
    
    language: {
        locale: 'en',
        src: 'location',
        locales: [{
            match: '/espanol',
            locale: 'sp'
        }]
    },
    
    exclude: {},
    
    zIndexPopup: 10000,
    
    ignoreWindowTopCheck: false,
    
    reverseButtons: true,
    
    ipexclude: 'fsr$ip',
    
    invite: {
        /* desktop */
        content: '<div id=\"fsrinvite\"><div id=\"fsrcontainer\"><div class=\"fsri_sitelogo\"><img src=\"{%baseHref%}sitelogo.gif\" alt=\"Site Logo\"></div><div class=\"fsri_fsrlogo\"><img src=\"{%baseHref%}fsrlogo.gif\" alt=\"Site Logo\"></div></div><div class=\"fsri_body\">\
      <div style=\"padding:0 0 8px 0;font-size:medium;font-weight:bold;\">We\'d welcome your feedback!</div>\
      <div style=\"padding:0 0 8px 0;\">Thank you for visiting our website. You have been selected to participate<br>in a brief customer satisfaction survey to let us know how we can improve<br>your experience.</div>\
      <div style=\"font-weight:bold;\">The survey is designed to measure your entire experience, please look for it at the <u>conclusion</u> of your visit.</div>\
      </div></div>',
        
        /*
         content: '<div id=\"fsrinvite\"><div id=\"fsrcontainer\"><div class=\"fsri_sitelogo\"><img src=\"{%baseHref%}sitelogo.gif\" alt=\"Site Logo\"></div><div class=\"fsri_fsrlogo\"><img src=\"{%baseHref%}fsrlogo.gif\" alt=\"Site Logo\"></div></div><div class=\"fsri_body\">\
         <div style=\"padding:0 0 8px 0;font-size:medium;font-weight:bold;\">We\'d welcome your feedback!</div>\
         <div style=\"padding:0 0 8px 0;\">Thank you for visiting our website. You have been selected to participate<br>in a brief customer satisfaction survey to let us know how we can improve<br>your experience.</div>\
         </div></div>',
         */
        /* mobile
         content: '<div id=\"fsrinvite\"><div id=\"fsrcontainer\"><div class=\"fsri_sitelogo\"><img src=\"{%baseHref%}sitelogo.gif\" alt=\"Site Logo\"></div><div class=\"fsri_fsrlogo\"><img src=\"{%baseHref%}fsrlogo.gif\" alt=\"Site Logo\"></div></div><div class=\"fsri_body\">\
         <div style=\"padding:0 0 5px 0;font-size:medium;font-weight:bold;\">We\'d welcome your feedback!</div>\
         <div style=\"padding:0 0 0 0;\">Thank you for visiting our website. You have been selected to participate in a brief customer satisfaction survey to let us know how we can improve your experience.</div>\
         </div></div>',
         */
        /* desktop */
        footer: '<div div id=\"fsrcontainer\"><div style=\"float:left;width:80%;font-size:8pt;text-align:left;line-height:12px;\">This survey is conducted by an independent company ForeSee,<br>on behalf of the site you are visiting.</div><div style=\"float:right;font-size:8pt;\"><a target="_blank" title="Validate TRUSTe privacy certification" href="//privacy-policy.truste.com/click-with-confidence/ctv/en/www.foreseeresults.com/seal_m"><img border=\"0\" src=\"{%baseHref%}truste.png\" alt=\"Validate TRUSTe Privacy Certification\"></a></div></div>',
        
        /* mobile
         footer: '<div div id=\"fsrcontainer\"><div style=\"float:left;width:50%;font-size:8pt;text-align:left;line-height:12px;\">Conducted by ForeSee</div><div style=\"float:right;font-size:8pt;text-align:right;\"><a target="_blank" title="Validate TRUSTe privacy certification" href="//privacy-policy.truste.com/click-with-confidence/ctv/en/www.foreseeresults.com/seal_m"><img style=\"width:50%;\" border=\"0\" src=\"{%baseHref%}truste.png\" alt=\"Validate TRUSTe Privacy Certification\"></a></div></div>',
         */
        exclude: {
            local: [/ncicancerbulletin/gi, /resultscancerbulletin.aspx/gi, /cb[a-zA-Z0-9-\.]*\.aspx/gi],
            referrer: []
        },
        include: {
            local: ['.']
        },
        
        /* desktop */
        width: '500',
        /* mobile
         width: {p: '260', l: '380'},
         text: {p: '100%', l: '70%'},
         */
        bgcolor: '#333',
        opacity: 0.7,
        x: 'center',
        y: 'center',
        delay: 0,
        timeout: 0,
        buttons: {
            accept: "Yes",
            decline: 'No'
        },
        hideOnClick: false,
        /* desktop */
        css: 'foresee-dhtml.css',
        /* mobile
         css: 'foresee-dhtml-mobile.css',
         */
        hide: [],
        type: 'dhtml',
        /* desktop */
        url: 'invite.html'
        /* mobile
         url: 'invite-mobile.html'
         */
    },
    
    tracker: {
        width: '690',
        height: '435',
        timeout: 3,
        adjust: true,
        alert: {
            enabled: true,
            message: 'The survey is now available.',
            locales: [{
                locale: 'sp',
                message: 'Su encuesta se encuentra ahora disponible.'
            }]
        },
        url: 'tracker.html',
        locales: [{
            locale: 'sp',
            url: 'tracker_sp.html'
        }]
    },
    
    survey: {
        width: 690,
        height: 600
    },
    
    qualifier: {
        width: '690',
        height: '500',
        bgcolor: '#333',
        opacity: 0.7,
        x: 'center',
        y: 'center',
        delay: 0,
        buttons: {
            accept: 'Continue'
        },
        hideOnClick: false,
        css: 'foresee-dhtml.css',
        url: 'qualifying.html',
        footer: '<div div id=\"fsrcontainer\"><div style=\"float:left;width:80%;font-size:8pt;text-align:left;line-height:12px;\">This survey is conducted by an independent company ForeSee,<br>on behalf of the site you are visiting.</div><div style=\"float:right;font-size:8pt;\"><a target="_blank" title="Validate TRUSTe privacy certification" href="//privacy-policy.truste.com/click-with-confidence/ctv/en/www.foreseeresults.com/seal_m"><img border=\"0\" src=\"{%baseHref%}truste.png\" alt=\"Validate TRUSTe Privacy Certification\"></a></div></div>'
    },
    
    cancel: {
        url: 'cancel.html',
        width: '690',
        height: '400'
    },
    
    pop: {
        what: 'survey',
        after: 'leaving-site',
        pu: false,
        tracker: true
    },
    
    meta: {
        referrer: true,
        terms: true,
        ref_url: true,
        url: true,
        url_params: false,
        user_agent: false
    },
    
    events: {
        enabled: true,
        id: true,
        codes: {
            purchase: 800,
            items: 801,
            dollars: 802,
            followup: 803,
            information: 804,
            content: 805
        },
        pd: 7,
        custom: {}
    },
    
    pool: 100,
    
    previous: false,
    
    analytics: {
        google: false
    },
    
    cpps: {},
    
    mode: 'hybrid'
};
