using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Calculator.Custom_Controls
{
    public class CustomTextBox : TextBox
    {
        private const int TextLimit = 21;

        public CustomTextBox()
        {
            TextChanged += CustomTextBox_TextChanged;
        }

        private void CustomTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // menghilangkan koma dari text
            string textWithoutCommas = Text.Replace(",", string.Empty);

            // Pangkas teks jika melebihi batas
            if (textWithoutCommas.Length > TextLimit)
            {
                textWithoutCommas = textWithoutCommas.Substring(0, TextLimit);
                Select(TextLimit, 0); // Move the cursor to the end of the truncated text
            }

            // Tambahkan koma setelah setiap tiga karakter
            string formattedText = AddCommas(textWithoutCommas);

            // Atur teks yang diformat kembali ke TextBox
            Text = formattedText;

            // Sesuaikan ukuran font jika teks melebihi lebar kontrol
            double availableWidth = ActualWidth - Padding.Left - Padding.Right;
            double textWidth = MeasureTextWidth(formattedText);

            if (textWidth > availableWidth)
            {
                double fontSize = FontSize;

                // Kurangi ukuran font hingga teks sesuai dengan lebar yang tersedia
                while (textWidth > availableWidth && fontSize > 1)
                {
                    fontSize--;
                    SetFontSize(fontSize);
                    textWidth = MeasureTextWidth(formattedText);
                }
            }

            // Atur FlowDirection ke RightToLeft untuk menampilkan teks dari kanan ke kiri
            FlowDirection = FlowDirection.RightToLeft;
        }

        private double MeasureTextWidth(string text)
        {
            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Foreground,
                new NumberSubstitution(),
                VisualTreeHelper.GetDpi(this).PixelsPerDip
            );

            return formattedText.Width;
        }

        private void SetFontSize(double fontSize)
        {
            FontSize = fontSize;
        }

        private string AddCommas(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Balikkan teks untuk menambahkan koma dari kanan ke kiri
            char[] charArray = text.ToCharArray();
            System.Array.Reverse(charArray);
            string reversedText = new string(charArray);

            // Tambahkan koma setelah setiap tiga karakter
            string formattedText = string.Empty;
            for (int i = 0; i < reversedText.Length; i += 3)
            {
                int length = System.Math.Min(3, reversedText.Length - i);
                formattedText += reversedText.Substring(i, length);
                if (i + length < reversedText.Length)
                    formattedText += ",";
            }

            // Membalikkan teks yang diformat kembali ke urutan aslinya
            charArray = formattedText.ToCharArray();
            System.Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}

