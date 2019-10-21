using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mostrarSegundaSeñal(false);
        }

        private void BtnGraficar_Click(object sender, RoutedEventArgs e)
        {
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);

            Señal señal;
            Señal segundaSeñal=null;
            Señal señalResultante;

            switch (cbTipoSeñal.SelectedIndex)
            {
                case 0: //Parabolica
                    señal = new SeñalParabolica();

                    break;
                case 1: //senoidal
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtAmplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtFase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtFrecuencia.Text);
                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);

                    break;
                case 2: //Exponencial
                    double alpha = double.Parse(((ConfiguracionSeñalExponencial)(panelConfiguracion.Children[0])).txtAlpha.Text);
                    señal = new SeñalExponencial(alpha);
                    break;
                case 3: //Audio
                    string rutaArchivo = ((ConfiguracionAudio)(panelConfiguracion.Children[0])).txtRutaArchivo.Text;
                    señal = new SeñalAudio(rutaArchivo);
                    txtTiempoInicial.Text = señal.TiempoInicial.ToString();
                    txtTiempoFinal.Text = señal.TiempoFinal.ToString();
                    txtFrecuenciaMuestreo.Text = señal.FrecuenciaMuestreo.ToString();
                    break;
                default:
                    señal = null;
                    break;
            }
            if (cbTipoSeñal.SelectedIndex != 3 && señal != null)
            {
                señal.TiempoInicial = tiempoInicial;
                señal.TiempoFinal = tiempoFinal;
                señal.FrecuenciaMuestreo = frecuenciaMuestreo;
                señal.constriurSeñal();

            }
            //Construir segunda señal si es necesario
            if(cbOperacion.SelectedIndex == 2)
            {
                switch (cbTipoSeñal_2.SelectedIndex)
                {
                    case 0: //Parabolica
                        segundaSeñal = new SeñalParabolica();

                        break;
                    case 1: //senoidal
                        double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion_2.Children[0])).txtAmplitud.Text);
                        double fase = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion_2.Children[0])).txtFase.Text);
                        double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion_2.Children[0])).txtFrecuencia.Text);
                        segundaSeñal = new SeñalSenoidal(amplitud, fase, frecuencia);

                        break;
                    case 2: //Exponencial
                        double alpha = double.Parse(((ConfiguracionSeñalExponencial)(panelConfiguracion_2.Children[0])).txtAlpha.Text);
                        segundaSeñal = new SeñalExponencial(alpha);
                        break;
                    case 3: //Audio
                        string rutaArchivo = ((ConfiguracionAudio)(panelConfiguracion_2.Children[0])).txtRutaArchivo.Text;
                        segundaSeñal = new SeñalAudio(rutaArchivo);
                        txtTiempoInicial.Text = segundaSeñal.TiempoInicial.ToString();
                        txtTiempoFinal.Text = segundaSeñal.TiempoFinal.ToString();
                        txtFrecuenciaMuestreo.Text = segundaSeñal.FrecuenciaMuestreo.ToString();
                        break;
                    default:
                        segundaSeñal = null;
                        break;
                }
                if(cbTipoSeñal_2.SelectedIndex !=3 && segundaSeñal != null)
                {
                    segundaSeñal.TiempoInicial = tiempoInicial;
                    segundaSeñal.TiempoFinal = tiempoFinal;
                    segundaSeñal.FrecuenciaMuestreo = frecuenciaMuestreo;
                    segundaSeñal.constriurSeñal();
                }
            }
          
            switch (cbOperacion.SelectedIndex)
            {
                case 0: //Escala de amplitud
                    double factorEscala = double.Parse(((OperacionEscalaAmplitud)
                        (panelConfiguracionOperacion.Children[0])).txtFactorEscala.Text);
                    señalResultante = Señal.escalarAmplitud(señal, factorEscala);
                    break;
                case 1: //Desplazamiento de Amplitud
                    double cantidadDesplazamiento = double.Parse(((OperacionDesplazamientoAmplitud)
                        (panelConfiguracionOperacion.Children[0])).txtDesplazamiento.Text);
                    señalResultante = Señal.desplazarAmplitud(señal, cantidadDesplazamiento);
                    break;
                case 2: //Multiplicacion de señales
                    señalResultante = Señal.multiplicarSeñales(señal, segundaSeñal);
                    break;
                case 3: //Escala exponencial
                    double exponente = double.Parse(((OperacionEscalaExponencial)
                        (panelConfiguracionOperacion.Children[0])).txtExponente.Text);
                    señalResultante = Señal.escalaExponencial(señal, exponente);
                    break;
                case 4: //transformada de Fourier
                    señalResultante = Señal.transformadaFourier(señal);
                    break;
                default:
                    señalResultante = null;
                    break;
            }
            //Elige entre la primera y la resultante
            double amplitudMaxima = (señal.AmplitudMaxima >= señalResultante.AmplitudMaxima) ? 
                señal.AmplitudMaxima : señalResultante.AmplitudMaxima;
            if(segundaSeñal != null)
            {
                //elige entre la mas grande de la 1ra y resultante y la segunda
                amplitudMaxima = (amplitudMaxima > segundaSeñal.AmplitudMaxima) ? amplitudMaxima : segundaSeñal.AmplitudMaxima;

            }



            plnGrafica.Points.Clear();
            plnGraficaResultante.Points.Clear();
            plnGrafica_2.Points.Clear();
            if(segundaSeñal != null)
            {
                foreach(var muestra in segundaSeñal.Muestras)
                {
                    plnGrafica_2.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
                }

            }

            foreach (Muestra muestra in señal.Muestras)
            {
                plnGrafica.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
            }
            foreach (Muestra muestra in señalResultante.Muestras)
            {
                plnGraficaResultante.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
            }
            //Original
            lblLimiteSuperior.Text = amplitudMaxima.ToString("F");
            lblLimiteInferior.Text = "-" + amplitudMaxima.ToString("F");
            plnEjeX.Points.Clear();
            plnEjeX.Points.Add(adaptarCoordenadas(tiempoInicial, 0.0, tiempoInicial, amplitudMaxima));
            plnEjeX.Points.Add(adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial, amplitudMaxima));
            plnEjeY.Points.Clear();
            plnEjeY.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));
            plnEjeY.Points.Add(adaptarCoordenadas(0.0, -amplitudMaxima, tiempoInicial, amplitudMaxima));
            //Resultado
            lblLimiteSuperiorResultante.Text = amplitudMaxima.ToString("F");
            lblLimiteInferiorResultado.Text = "-" + amplitudMaxima.ToString("F");
            plnEjeXResultante.Points.Clear();
            plnEjeXResultante.Points.Add(adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial, amplitudMaxima));
            plnEjeXResultante.Points.Add(adaptarCoordenadas(tiempoInicial, 0.0, tiempoInicial, amplitudMaxima));
            plnEjeYResultante.Points.Clear();
            plnEjeYResultante.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));
            plnEjeYResultante.Points.Add(adaptarCoordenadas(0.0, -amplitudMaxima, tiempoInicial, amplitudMaxima));


        }
        public Point adaptarCoordenadas(double x, double y, double tiempoInicial, double amplitudMaxima)
        {
            return new Point((x-tiempoInicial) * scrGrafica.Width, (-1 * (y * ((( scrGrafica.Height / 2.0) - 25)/amplitudMaxima ) )) + (scrGrafica.Height / 2.0));
        }

        private void CbTipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion.Children.Clear();
            switch (cbTipoSeñal.SelectedIndex)
            {
                case 0: //Parabolica
                    break;
                case 1: //Senoidal
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 2:
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalExponencial());
                    break;
                case 3:
                    panelConfiguracion.Children.Add(new ConfiguracionAudio());
                    break;
                default:
                    break;
            }
        }

        private void CbOperacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracionOperacion.Children.Clear();
            mostrarSegundaSeñal(false);
            switch (cbOperacion.SelectedIndex)
            {
                case 0: //escala de amplitud
                    panelConfiguracionOperacion.Children.Add(new OperacionEscalaAmplitud());
                    break;
                case 1://desplazamineto amplitud
                    panelConfiguracionOperacion.Children.Add(new OperacionDesplazamientoAmplitud());
                    break;
                case 2://multiplicacion de señales
                    mostrarSegundaSeñal(true);
                    break;
                case 3: //esacala exponencial
                    panelConfiguracionOperacion.Children.Add(new OperacionEscalaExponencial());
                    break;
                default:
                    break;
            }
        }

        private void CbTipoSeñal_2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion_2.Children.Clear();
            switch (cbTipoSeñal_2.SelectedIndex)
            {
                case 0: //Parabolica
                    break;
                case 1: //Senoidal
                    panelConfiguracion_2.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 2:
                    panelConfiguracion_2.Children.Add(new ConfiguracionSeñalExponencial());
                    break;
                case 3:
                    panelConfiguracion_2.Children.Add(new ConfiguracionAudio());
                    break;
                default:
                    break;
            }
        }
        void mostrarSegundaSeñal(bool mostrar)
        {
            if (mostrar)
            {
                lblTipoSeñal2.Visibility = Visibility.Visible;
                cbTipoSeñal_2.Visibility = Visibility.Visible;
                panelConfiguracion_2.Visibility = Visibility.Visible;
            }
            else
            {
                lblTipoSeñal2.Visibility = Visibility.Hidden;
                cbTipoSeñal_2.Visibility = Visibility.Hidden;
                panelConfiguracion_2.Visibility = Visibility.Hidden;
            }
        }
        //Teorema de muestreo fs=2fmax+1
        //indice a Hz (i * fm) / N
    }
}

