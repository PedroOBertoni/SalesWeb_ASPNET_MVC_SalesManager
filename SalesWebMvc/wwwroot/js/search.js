function setupDateValidation(formId, minDateId, maxDateId, errorDivId, checkUrl) {
    const form = document.getElementById(formId);
    const minDateInput = document.getElementById(minDateId);
    const maxDateInput = document.getElementById(maxDateId);
    const errorDiv = document.getElementById(errorDivId);

    if (!form || !minDateInput || !maxDateInput || !errorDiv) {
        console.warn(`setupDateValidation: Um ou mais elementos não encontrados.
            Form ID: ${formId} (${form ? 'Encontrado' : 'Não encontrado'})
            MinDate ID: ${minDateId} (${minDateInput ? 'Encontrado' : 'Não encontrado'})
            MaxDate ID: ${maxDateId} (${maxDateInput ? 'Encontrado' : 'Não encontrado'})
            ErrorDiv ID: ${errorDivId} (${errorDiv ? 'Encontrado' : 'Não encontrado'})
        `);
        return;
    }

    form.addEventListener("submit", async function (event) {
        event.preventDefault(); // Impede o envio padrão do formulário

        let minDateValue = minDateInput.value;
        let maxDateValue = maxDateInput.value;

        function parseDateString(dateStr) {
            // Verifica se a string tem o formato dd/mm/aaaa
            if (!dateStr || dateStr.length !== 10) return null;
            const parts = dateStr.split('/');
            if (parts.length === 3) {
                const day = parseInt(parts[0], 10);
                const month = parseInt(parts[1], 10) - 1; // Mês é baseado em 0 (janeiro = 0)
                const year = parseInt(parts[2], 10);

                // Cria uma data e verifica se os componentes são os mesmos para validar datas inválidas (ex: 31/02)
                const date = new Date(year, month, day);
                if (date.getFullYear() === year && date.getMonth() === month && date.getDate() === day) {
                    return date;
                }
            }
            return null;
        }

        const minDateParsed = parseDateString(minDateValue);
        const maxDateParsed = parseDateString(maxDateValue);
        const today = new Date();
        today.setHours(0, 0, 0, 0); // Zera a hora para comparação apenas de data

        errorDiv.style.display = "none";
        errorDiv.textContent = "";

        // Validação de preenchimento e formato
        if (!minDateValue || !maxDateValue || !minDateParsed || !maxDateParsed) {
            errorDiv.textContent = "Preencha ambas as datas no formato correto (DD/MM/AAAA) antes de buscar.";
            errorDiv.style.display = "block";
            return;
        }

        // Validação de data futura
        if (minDateParsed > today) {
            errorDiv.textContent = "A data inicial não pode ser no futuro.";
            errorDiv.style.display = "block";
            // Não limpa o campo, permite que o usuário corrija
            return;
        }

        if (maxDateParsed > today) {
            errorDiv.textContent = "A data final não pode ser no futuro.";
            errorDiv.style.display = "block";
            // Não limpa o campo, permite que o usuário corrija
            return;
        }

        // Validação de ordem das datas
        if (maxDateParsed < minDateParsed) {
            errorDiv.textContent = "A data final não pode ser anterior à data inicial.";
            errorDiv.style.display = "block";
            // Não limpa os campos, permite que o usuário corrija
            return;
        }

        try {
            // Formata as datas para o padrão ISO (YYYY-MM-DD) para passar na URL
            const formattedMinDate = minDateParsed.toISOString().split('T')[0];
            const formattedMaxDate = maxDateParsed.toISOString().split('T')[0];

            // Realiza a chamada AJAX para o endpoint de verificação
            const response = await fetch(`${checkUrl}?minDate=${formattedMinDate}&maxDate=${formattedMaxDate}`);

            if (!response.ok) {
                // Captura erros de rede ou status HTTP não-2xx
                throw new Error(`HTTP error! status: ${response.status} - ${response.statusText}`);
            }

            const json = await response.json();

            if (!json.success) {
                // Se a verificação retornar false (nenhum registro)
                errorDiv.textContent = "Nenhuma venda foi registrada nesse período.";
                errorDiv.style.display = "block";
            } else {
                form.submit();
            }
        } catch (error) {
            console.error("Erro na verificação de registros:", error);
            errorDiv.textContent = "Erro ao verificar registros. Por favor, tente novamente.";
            errorDiv.style.display = "block";
        }
    });
}