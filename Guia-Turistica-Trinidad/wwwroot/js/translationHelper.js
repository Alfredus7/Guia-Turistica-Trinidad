// translationHelper.js - Funciones optimizadas de traducción para el sistema de turismo

const translationHelper = (() => {
    // Diccionario expandido con palabras comunes en turismo
    const dictionary = {
        // Lugares/edificios
        'catedral': { en: 'Cathedral', pt: 'Catedral' },
        'iglesia': { en: 'Church', pt: 'Igreja' },
        'plaza': { en: 'Square', pt: 'Praça' },
        'parque': { en: 'Park', pt: 'Parque' },
        'museo': { en: 'Museum', pt: 'Museu' },
        'mercado': { en: 'Market', pt: 'Mercado' },
        'playa': { en: 'Beach', pt: 'Praia' },
        'montaña': { en: 'Mountain', pt: 'Montanha' },
        'cerro': { en: 'Hill', pt: 'Colina' },
        'río': { en: 'River', pt: 'Rio' },
        'lago': { en: 'Lake', pt: 'Lago' },
        'pueblo': { en: 'Town', pt: 'Cidade' },
        'ciudad': { en: 'City', pt: 'Cidade' },
        'avenida': { en: 'Avenue', pt: 'Avenida' },
        'calle': { en: 'Street', pt: 'Rua' },
        'puente': { en: 'Bridge', pt: 'Ponte' },
        'puerto': { en: 'Port', pt: 'Porto' },
        'terminal': { en: 'Terminal', pt: 'Terminal' },
        'estación': { en: 'Station', pt: 'Estação' },

        // Atributos religiosos
        'santísima': { en: 'Holy', pt: 'Santíssima' },
        'trinidad': { en: 'Trinity', pt: 'Trindade' },
        'virgen': { en: 'Virgin', pt: 'Virgem' },
        'maría': { en: 'Mary', pt: 'Maria' },
        'san': { en: 'Saint', pt: 'São' },
        'santo': { en: 'Saint', pt: 'Santo' },
        'santa': { en: 'Saint', pt: 'Santa' },

        // Atributos descriptivos
        'histórico': { en: 'Historical', pt: 'Histórico' },
        'histórica': { en: 'Historical', pt: 'Histórica' },
        'cultural': { en: 'Cultural', pt: 'Cultural' },
        'natural': { en: 'Natural', pt: 'Natural' },
        'tradicional': { en: 'Traditional', pt: 'Tradicional' },
        'antiguo': { en: 'Ancient', pt: 'Antigo' },
        'antigua': { en: 'Ancient', pt: 'Antiga' },
        'importante': { en: 'Important', pt: 'Importante' },
        'hermoso': { en: 'Beautiful', pt: 'Belo' },
        'hermosa': { en: 'Beautiful', pt: 'Bela' },
        'bonito': { en: 'Pretty', pt: 'Bonito' },
        'bonita': { en: 'Pretty', pt: 'Bonita' },
        'famoso': { en: 'Famous', pt: 'Famoso' },
        'famosa': { en: 'Famous', pt: 'Famosa' },
        'turístico': { en: 'Tourist', pt: 'Turístico' },
        'turística': { en: 'Tourist', pt: 'Turística' },
        'principal': { en: 'Main', pt: 'Principal' },
        'central': { en: 'Central', pt: 'Central' },
        'nacional': { en: 'National', pt: 'Nacional' },
        'regional': { en: 'Regional', pt: 'Regional' },
        'local': { en: 'Local', pt: 'Local' },
        'público': { en: 'Public', pt: 'Público' },
        'pública': { en: 'Public', pt: 'Pública' },
        'privado': { en: 'Private', pt: 'Privado' },
        'privada': { en: 'Private', pt: 'Privada' },

        // Tipos de sitios comunes
        'mirador': { en: 'Viewpoint', pt: 'Mirante' },
        'balneario': { en: 'Spa', pt: 'Balneário' },
        'termas': { en: 'Hot Springs', pt: 'Termas' },
        'cascada': { en: 'Waterfall', pt: 'Cachoeira' },
        'caída': { en: 'Falls', pt: 'Queda' },
        'bosque': { en: 'Forest', pt: 'Bosque' },
        'selva': { en: 'Jungle', pt: 'Selva' },
        'reserva': { en: 'Reserve', pt: 'Reserva' },
        'área': { en: 'Area', pt: 'Área' },
        'zona': { en: 'Zone', pt: 'Zona' },

        // Preposiciones y artículos
        'de la': { en: 'of the', pt: 'da' },
        'del': { en: 'of the', pt: 'do' },
        'y': { en: 'and', pt: 'e' },
        'o': { en: 'or', pt: 'ou' },
        'la': { en: 'the', pt: 'a' },
        'el': { en: 'the', pt: 'o' },
        'los': { en: 'the', pt: 'os' },
        'las': { en: 'the', pt: 'as' },
        'un': { en: 'a', pt: 'um' },
        'una': { en: 'a', pt: 'uma' },
        'en': { en: 'in', pt: 'em' },
        'con': { en: 'with', pt: 'com' },
        'para': { en: 'for', pt: 'para' },
        'por': { en: 'by', pt: 'por' },
        'sobre': { en: 'about', pt: 'sobre' },
        'entre': { en: 'between', pt: 'entre' },
        'desde': { en: 'from', pt: 'desde' },
        'hasta': { en: 'until', pt: 'até' }
    };

    // Palabras que NO deben traducirse (nombres propios, lugares específicos)
    const noTranslate = [
        'trinidad', 'beni', 'bolivia', 'amazonas', 'andes',
        'mamoré', 'ibera', 'ibero', 'jesuítico', 'guaraní'
    ];

    // Verificar si una palabra está en la lista de no traducción
    const shouldNotTranslate = (word) => {
        return noTranslate.some(noTransWord =>
            word.toLowerCase().includes(noTransWord.toLowerCase())
        );
    };

    // Traducir texto del español a otros idiomas
    const translateText = (text) => {
        if (!text || typeof text !== 'string') return { en: '', pt: '' };

        let englishText = text;
        let portugueseText = text;

        // Ordenar las palabras por longitud (de más larga a más corta)
        const sortedWords = Object.keys(dictionary).sort((a, b) => b.length - a.length);

        sortedWords.forEach(word => {
            // Crear expresión regular que busque la palabra como palabra completa
            const regex = new RegExp(`\\b${word}\\b`, 'gi');
            const matches = text.match(regex);

            if (matches) {
                // Verificar cada coincidencia individualmente
                matches.forEach(match => {
                    // Si la palabra coincidente NO está en la lista de no traducción
                    if (!shouldNotTranslate(match)) {
                        englishText = englishText.replace(
                            new RegExp(`\\b${match}\\b`, 'g'),
                            dictionary[word].en
                        );
                        portugueseText = portugueseText.replace(
                            new RegExp(`\\b${match}\\b`, 'g'),
                            dictionary[word].pt
                        );
                    }
                });
            }
        });

        // Capitalizar apropiadamente
        englishText = capitalizeWords(englishText);
        portugueseText = capitalizeWords(portugueseText);

        return { en: englishText, pt: portugueseText };
    };

    // Traducir descripción
    const translateDescription = (text, targetLang) => {
        if (!text || text.trim() === '') return '';

        let translatedText = text;
        const sortedWords = Object.keys(dictionary).sort((a, b) => b.length - a.length);

        sortedWords.forEach(word => {
            const regex = new RegExp(`\\b${word}\\b`, 'gi');
            const matches = text.match(regex);

            if (matches) {
                matches.forEach(match => {
                    if (!shouldNotTranslate(match)) {
                        const replacement = targetLang === 'en' ? dictionary[word].en : dictionary[word].pt;
                        translatedText = translatedText.replace(
                            new RegExp(`\\b${match}\\b`, 'g'),
                            replacement
                        );
                    }
                });
            }
        });

        translatedText = adjustGrammar(translatedText, targetLang);
        return capitalizeWords(translatedText);
    };

    // Ajustar gramática básica
    const adjustGrammar = (text, lang) => {
        if (lang === 'en') {
            text = text.replace(/\bla\b/gi, 'the');
            text = text.replace(/\bel\b/gi, 'the');
            text = text.replace(/\blos\b/gi, 'the');
            text = text.replace(/\blas\b/gi, 'the');
            text = text.replace(/\bun\b/gi, 'a');
            text = text.replace(/\buna\b/gi, 'a');
            text = text.replace(/\bde la\b/gi, 'of the');
            text = text.replace(/\bdel\b/gi, 'of the');
            text = text.replace(/\by\b/gi, 'and');
            text = text.replace(/\bo\b/gi, 'or');
        } else if (lang === 'pt') {
            text = text.replace(/\bla\b/gi, 'a');
            text = text.replace(/\bel\b/gi, 'o');
            text = text.replace(/\blos\b/gi, 'os');
            text = text.replace(/\blas\b/gi, 'as');
            text = text.replace(/\bun\b/gi, 'um');
            text = text.replace(/\buna\b/gi, 'uma');
            text = text.replace(/\bde la\b/gi, 'da');
            text = text.replace(/\bdel\b/gi, 'do');
            text = text.replace(/\by\b/gi, 'e');
            text = text.replace(/\bo\b/gi, 'ou');
        }
        return text;
    };

    // Capitalizar palabras
    const capitalizeWords = (text) => {
        return text.toLowerCase()
            .replace(/\b\w/g, char => char.toUpperCase())
            .replace(/\bDe\b/g, 'de')
            .replace(/\bLa\b/g, 'la')
            .replace(/\bEl\b/g, 'el')
            .replace(/\bY\b/g, 'y')
            .replace(/\bO\b/g, 'o');
    };

    // Generar descripción genérica mejorada
    const generateGenericDescription = (spanishName, translatedName, lang) => {
        const baseName = translatedName || spanishName;

        const descriptions = {
            'es': `El "${spanishName}" es un destacado destino turístico ubicado en Trinidad, departamento del Beni, Bolivia. Este sitio ofrece experiencias únicas para visitantes nacionales e internacionales, combinando una rica herencia cultural con la impresionante belleza natural de la región amazónica. Es un lugar ideal para explorar la historia, tradiciones y biodiversidad única de esta área.`,
            'en': `The "${baseName}" is a prominent tourist destination located in Trinidad, Beni department, Bolivia. This site offers unique experiences for national and international visitors, combining a rich cultural heritage with the stunning natural beauty of the Amazon region. It is an ideal place to explore the history, traditions, and unique biodiversity of this area.`,
            'pt': `O "${baseName}" é um destacado destino turístico localizado em Trinidad, departamento de Beni, Bolívia. Este local oferece experiências únicas para visitantes nacionais e internacionais, combinando um rico patrimônio cultural com a deslumbrante beleza natural da região amazônica. É um lugar ideal para explorar a história, tradições e biodiversidade única desta área.`
        };

        return descriptions[lang] || descriptions['en'];
    };

    // Sugerir traducciones para sitios turísticos (CORREGIDO)
    const suggestTouristSiteTranslations = () => {
        console.log('🚀 Iniciando traducción para sitio turístico...');

        const spanishName = getElementValue('nombreInput');
        const spanishDesc = getElementValue('descripcionInput');

        if (!spanishName) {
            showGlobalToast('Por favor, primero ingresa el nombre en español', 'warning');
            focusElement('nombreInput');
            return;
        }

        console.log('📝 Traduciendo nombre:', spanishName);
        const translated = translateText(spanishName);

        // Actualizar NOMBRES (siempre)
        updateFieldIfEmpty('nombreInglesInput', translated.en);
        updateFieldIfEmpty('nombrePortuguesInput', translated.pt);

        // VERIFICAR SI HAY DESCRIPCIÓN EN ESPAÑOL
        console.log('📝 Descripción española actual:', spanishDesc ? 'Sí' : 'No');

        // SIEMPRE generar descripciones genéricas para TODOS los idiomas
        // Esto es lo mismo que haces en tipos de sitio
        const genericDescEs = generateGenericDescription(spanishName, spanishName, 'es');
        const genericDescEn = generateGenericDescription(spanishName, translated.en, 'en');
        const genericDescPt = generateGenericDescription(spanishName, translated.pt, 'pt');

        console.log('✅ Generando descripciones genéricas para todos los idiomas');

        // Actualizar campos - ESPECIAL: para español, solo si está vacío
        updateFieldIfEmpty('descripcionInput', genericDescEs);

        // Para inglés y portugués, también solo si están vacíos
        updateFieldIfEmpty('descripcionInglesInput', genericDescEn);
        updateFieldIfEmpty('descripcionPortuguesInput', genericDescPt);

        showGlobalToast('Traducciones sugeridas generadas con éxito', 'success');
        console.log('✅ Traducciones completadas');
    };

    // Sugerir traducciones para tipos de sitios (MANTENIENDO TU LÓGICA ORIGINAL)
    const suggestSiteTypeTranslations = () => {
        console.log('🚀 Iniciando traducción para tipo de sitio...');

        const spanishName = getElementValue('Nombre');
        const spanishDesc = getElementValue('Descripcion');

        if (!spanishName) {
            showGlobalToast('Por favor, primero ingresa el nombre en español', 'warning');
            focusElement('Nombre');
            return;
        }

        const translated = translateText(spanishName);

        // Actualizar nombres
        updateFieldIfEmpty('NombreIngles', translated.en);
        updateFieldIfEmpty('NombrePortugues', translated.pt);

        // SIEMPRE generar descripciones genéricas para TODOS los idiomas
        const genericDescEs = `Sitios turísticos del tipo "${spanishName}" que ofrecen experiencias únicas para los visitantes en Trinidad, Beni. Estos lugares combinan patrimonio cultural, belleza natural y actividades recreativas que reflejan la identidad de la región amazónica boliviana.`;
        const genericDescEn = `Tourist sites of the "${translated.en || spanishName}" type offering unique experiences for visitors in Trinidad, Beni. These places combine cultural heritage, natural beauty, and recreational activities that reflect the identity of the Bolivian Amazon region.`;
        const genericDescPt = `Sítios turísticos do tipo "${translated.pt || spanishName}" que oferecem experiências únicas para os visitantes em Trinidad, Beni. Estes locais combinam patrimônio cultural, beleza natural e atividades recreativas que refletem a identidade da região amazônica boliviana.`;

        // Actualizar campos - para español, inglés y portugués
        updateFieldIfEmpty('Descripcion', genericDescEs);
        updateFieldIfEmpty('DescripcionIngles', genericDescEn);
        updateFieldIfEmpty('DescripcionPortugues', genericDescPt);

        showGlobalToast('Traducciones sugeridas generadas con éxito', 'success');
    };

    // Método unificador para detectar y ejecutar la traducción correcta
    const suggestTranslations = () => {
        console.log('🔍 Detectando tipo de formulario...');

        if (document.getElementById('nombreInput')) {
            console.log('✅ Formulario de sitio turístico detectado');
            suggestTouristSiteTranslations();
        } else if (document.getElementById('Nombre')) {
            console.log('✅ Formulario de tipo de sitio detectado');
            suggestSiteTypeTranslations();
        } else {
            showGlobalToast('No se pudo detectar el tipo de formulario. Asegúrate de estar en la página correcta.', 'warning');
            console.error('❌ No se pudo detectar el formulario');
        }
    };

    // Helper functions
    const getElementValue = (id) => {
        const element = document.getElementById(id);
        return element ? element.value.trim() : '';
    };

    const updateFieldIfEmpty = (id, value) => {
        const element = document.getElementById(id);
        if (element && value) {
            // Solo actualizar si el campo está vacío o casi vacío
            const currentValue = element.value.trim();
            if (currentValue === '' || currentValue.length < 10) {
                element.value = value;
                // Disparar eventos para que se actualicen los contadores
                element.dispatchEvent(new Event('input', { bubbles: true }));
                element.dispatchEvent(new Event('change', { bubbles: true }));
                console.log(`✅ Campo ${id} actualizado (estaba vacío)`);
            } else {
                console.log(`⚠️ Campo ${id} NO actualizado (ya tenía contenido)`);
            }
        }
    };

    const updateField = (id, value) => {
        const element = document.getElementById(id);
        if (element && value !== undefined && value !== null) {
            element.value = value;
            element.dispatchEvent(new Event('input', { bubbles: true }));
            element.dispatchEvent(new Event('change', { bubbles: true }));
        }
    };

    const focusElement = (id) => {
        const element = document.getElementById(id);
        if (element) element.focus();
    };

    // Mostrar toast global
    const showGlobalToast = (message, type = 'info') => {
        // Remover toasts existentes
        document.querySelectorAll('.toast').forEach(toast => toast.remove());

        const icons = {
            'success': 'check-circle',
            'danger': 'exclamation-circle',
            'warning': 'exclamation-triangle',
            'info': 'info-circle'
        };

        const toastHtml = `
            <div class="toast align-items-center text-bg-${type} border-0 fade show"
                 role="alert" style="position: fixed; top: 20px; right: 20px; z-index: 9999;">
                <div class="d-flex">
                    <div class="toast-body">
                        <i class="fas fa-${icons[type] || 'info-circle'} me-2"></i>
                        ${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" 
                            onclick="this.parentElement.parentElement.remove()"></button>
                </div>
            </div>
        `;

        document.body.insertAdjacentHTML('beforeend', toastHtml);

        setTimeout(() => {
            const toast = document.querySelector('.toast');
            if (toast) toast.remove();
        }, 5000);
    };

    // Métodos públicos
    return {
        suggestTranslations, // Método unificador
        suggestTouristSiteTranslations,
        suggestSiteTypeTranslations,
        translateText,
        translateDescription,
        showGlobalToast,
        generateGenericDescription,

        // Para debugging
        getDictionary: () => dictionary,
        getNoTranslateList: () => noTranslate
    };
})();

// Exportar al ámbito global
window.translationHelper = translationHelper;

// Para depuración
console.log('✅ Translation Helper cargado y optimizado correctamente');
console.log('📚 Diccionario cargado:', Object.keys(translationHelper.getDictionary()).length, 'palabras');