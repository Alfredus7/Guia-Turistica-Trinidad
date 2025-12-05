// wwwroot/js/imageUrlValidator.js
// Módulo global para validación de URLs de imágenes

const imageUrlValidator = {
    // Patrones de extensiones de imagen
    imagePatterns: [
        /\.(jpeg|jpg|jfif|pjpeg|pjp)$/i,
        /\.(png)$/i,
        /\.(gif)$/i,
        /\.(bmp)$/i,
        /\.(webp)$/i,
        /\.(svg)$/i,
        /\.(avif)$/i
    ],

    // Validar si una URL es una URL de imagen válida
    isValidImageUrl(url) {
        if (!url) return false;

        try {
            const urlObj = new URL(url);
            return this.imagePatterns.some(pattern => pattern.test(urlObj.pathname));
        } catch {
            return false;
        }
    },

    // Validar y cargar una imagen para vista previa
    validateAndPreviewImage(url, imageElement, callback) {
        return new Promise((resolve) => {
            if (!this.isValidImageUrl(url)) {
                resolve({ valid: false, message: 'La URL no parece ser una imagen válida' });
                return;
            }

            const testImage = new Image();
            testImage.onload = () => {
                if (imageElement) {
                    imageElement.src = url;
                    imageElement.onerror = () => {
                        imageElement.src = 'https://via.placeholder.com/400/dc3545/ffffff?text=Error+cargando';
                    };
                }
                resolve({ valid: true, message: 'Imagen válida' });
            };

            testImage.onerror = () => {
                resolve({ valid: false, message: 'La imagen no se pudo cargar (puede ser temporal)' });
            };

            testImage.src = url;
        });
    },

    // Mostrar feedback de validación
    showUrlFeedback(message, type, feedbackElement, loadingIconElement, statusElement) {
        if (loadingIconElement) {
            loadingIconElement.className = 'fas fa-spinner fa-spin me-2 d-none';
        }

        let feedbackClass = 'alert alert-light border d-flex align-items-center';
        let statusClass = 'badge badge-success px-3 py-2';
        let statusHtml = '<i class="fas fa-image me-1"></i>Esperando URL';

        switch (type) {
            case 'success':
                feedbackClass = 'alert alert-success border d-flex align-items-center';
                statusClass = 'badge badge-success px-3 py-2';
                statusHtml = '<i class="fas fa-check-circle me-1"></i>Imagen válida';
                break;
            case 'warning':
                feedbackClass = 'alert alert-warning border d-flex align-items-center';
                statusClass = 'badge badge-warning px-3 py-2';
                statusHtml = '<i class="fas fa-exclamation-triangle me-1"></i>Advertencia';
                break;
            case 'danger':
                feedbackClass = 'alert alert-danger border d-flex align-items-center';
                statusClass = 'badge badge-danger px-3 py-2';
                statusHtml = '<i class="fas fa-times-circle me-1"></i>Error en URL';
                break;
        }

        if (feedbackElement) {
            feedbackElement.className = feedbackClass;
            feedbackElement.classList.remove('d-none');
            const textSpan = feedbackElement.querySelector('#urlFeedbackText') || feedbackElement.querySelector('span');
            if (textSpan) {
                textSpan.textContent = message;
            }
        }

        if (statusElement) {
            statusElement.className = statusClass;
            statusElement.innerHTML = statusHtml;
        }

        if (type === 'success' && feedbackElement) {
            setTimeout(() => {
                feedbackElement.classList.add('d-none');
            }, 3000);
        }
    },

    // Configurar validación en tiempo real para un input de URL de imagen
    setupRealTimeValidation(inputElement, previewElement, feedbackElement, loadingIconElement, statusElement) {
        let debounceTimer;

        inputElement.addEventListener('input', function () {
            clearTimeout(debounceTimer);
            const url = this.value.trim();

            if (!url) {
                if (previewElement) {
                    previewElement.src = 'https://via.placeholder.com/400/6c757d/ffffff?text=Preview';
                }
                if (feedbackElement) {
                    feedbackElement.classList.add('d-none');
                }
                this.classList.remove('is-valid', 'is-invalid');
                if (statusElement) {
                    statusElement.className = 'badge badge-success px-3 py-2';
                    statusElement.innerHTML = '<i class="fas fa-image me-1"></i>Esperando URL';
                }
                return;
            }

            if (feedbackElement) {
                feedbackElement.classList.remove('d-none');
                feedbackElement.className = 'alert alert-light border d-flex align-items-center';
                if (loadingIconElement) {
                    loadingIconElement.className = 'fas fa-spinner fa-spin me-2 text-info';
                }
                const textSpan = feedbackElement.querySelector('#urlFeedbackText') || feedbackElement.querySelector('span');
                if (textSpan) {
                    textSpan.textContent = 'Validando URL...';
                }
            }

            debounceTimer = setTimeout(() => {
                this.validateImageUrl(url, previewElement, feedbackElement, loadingIconElement, statusElement);
            }, 800);
        });

        inputElement.addEventListener('blur', function () {
            if (this.value.trim()) {
                this.validateImageUrl(this.value.trim(), previewElement, feedbackElement, loadingIconElement, statusElement);
            }
        });

        // Añadir método de validación al input
        inputElement.validateImageUrl = function (url, previewElement, feedbackElement, loadingIconElement, statusElement) {
            if (!imageUrlValidator.isValidImageUrl(url)) {
                imageUrlValidator.showUrlFeedback('❌ URL inválida - Formato incorrecto', 'danger', feedbackElement, loadingIconElement, statusElement);
                if (previewElement) {
                    previewElement.src = 'https://via.placeholder.com/400/dc3545/ffffff?text=URL+inválida';
                }
                this.classList.remove('is-valid');
                this.classList.add('is-invalid');
                return;
            }

            imageUrlValidator.validateAndPreviewImage(url, previewElement).then(result => {
                if (result.valid) {
                    imageUrlValidator.showUrlFeedback('✅ Imagen válida - Vista previa actualizada', 'success', feedbackElement, loadingIconElement, statusElement);
                    this.classList.remove('is-invalid');
                    this.classList.add('is-valid');
                } else {
                    imageUrlValidator.showUrlFeedback('⚠️ ' + result.message, 'warning', feedbackElement, loadingIconElement, statusElement);
                    if (previewElement) {
                        previewElement.src = 'https://via.placeholder.com/400/ffc107/000000?text=Error+carga';
                    }
                    this.classList.remove('is-valid');
                    this.classList.add('is-invalid');
                }
            });
        };
    }
};