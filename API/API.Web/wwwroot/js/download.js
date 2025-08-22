// Funkcja do pobierania plików
window.downloadFile = function (fileName, base64Content, contentType) {
  // Konwertuj base64 na blob
  const binaryString = atob(base64Content);
  const bytes = new Uint8Array(binaryString.length);
  for (let i = 0; i < binaryString.length; i++) {
    bytes[i] = binaryString.charCodeAt(i);
  }

  const blob = new Blob([bytes], { type: contentType });

  // Utwórz link do pobrania
  const url = URL.createObjectURL(blob);
  const link = document.createElement("a");
  link.href = url;
  link.download = fileName;

  // Dodaj do DOM, kliknij i usuń
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);

  // Zwolnij pamięć
  URL.revokeObjectURL(url);
};

// Funkcja do generowania PDF z HTML
window.generatePDFFromHTML = function (htmlContent, fileName) {
  return new Promise((resolve, reject) => {
    try {
      // Utwórz tymczasowy element div
      const tempDiv = document.createElement("div");
      tempDiv.innerHTML = htmlContent;
      tempDiv.style.position = "absolute";
      tempDiv.style.left = "-9999px";
      tempDiv.style.top = "-9999px";
      tempDiv.style.width = "210mm"; // A4 width
      tempDiv.style.backgroundColor = "white";
      document.body.appendChild(tempDiv);

      // Użyj html2canvas do konwersji HTML na canvas
      html2canvas(tempDiv, {
        scale: 2,
        useCORS: true,
        allowTaint: true,
        backgroundColor: "#ffffff",
        width: 794, // A4 width in pixels at 96 DPI
        height: 1123, // A4 height in pixels at 96 DPI
      })
        .then((canvas) => {
          // Usuń tymczasowy element
          document.body.removeChild(tempDiv);

          // Konwertuj canvas na obraz
          const imgData = canvas.toDataURL("image/png");

          // Utwórz PDF używając jsPDF
          const { jsPDF } = window.jspdf;
          const pdf = new jsPDF("p", "mm", "a4");

          // Oblicz wymiary dla A4
          const imgWidth = 210;
          const pageHeight = 295;
          const imgHeight = (canvas.height * imgWidth) / canvas.width;
          let heightLeft = imgHeight;
          let position = 0;

          // Dodaj pierwszą stronę
          pdf.addImage(imgData, "PNG", 0, position, imgWidth, imgHeight);
          heightLeft -= pageHeight;

          // Dodaj kolejne strony jeśli potrzebne
          while (heightLeft >= 0) {
            position = heightLeft - imgHeight;
            pdf.addPage();
            pdf.addImage(imgData, "PNG", 0, position, imgWidth, imgHeight);
            heightLeft -= pageHeight;
          }

          // Zapisz PDF
          pdf.save(fileName);
          resolve(true);
        })
        .catch((error) => {
          document.body.removeChild(tempDiv);
          reject(error);
        });
    } catch (error) {
      reject(error);
    }
  });
};
