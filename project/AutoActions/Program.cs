using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoActions
{
    class Program
    {
        private const uint ENABLE_QUICK_EDIT = 0x0040;
        private const int STD_INPUT_HANDLE = -10;

        private static readonly string AppPath = AppDomain.CurrentDomain.BaseDirectory;

        private static Stopwatch _stopwatch = null;

        private static bool _isRunning = false;
        private static bool _isReading = false;

        private static Keys _currentHotkey = Keys.None;
        private static List<Instruction> _currentScript = null;
        private static string _currentScriptName = "Sin cargar";

        [ STAThread ]
        static void Main( string[ ] args )
        {
            DisableQuickEdit( );

            Console.CursorVisible = false;

            Console.SetWindowSize( 80, 15 );
            Console.SetBufferSize( 80, 15 );

            Console.Title = "Auto Actions";

            _stopwatch = new Stopwatch( );

            var keyboard = new Open.WinKeyboardHook.KeyboardInterceptor( );

            keyboard.KeyDown += OnKeyDownEvent;
            keyboard.StartCapturing( );

            Task.Run( StartThink );

            Application.Run( );

            keyboard.StopCapturing( );
        }

        private static void StartThink( )
        {
            while ( true )
            {
                Console.Clear( );

                Console.WriteLine( "****************************************" );
                Console.WriteLine( "         Auto Actions v 1.0 b" );
                Console.WriteLine( "****************************************\n" );
                
                if ( _isRunning )
                {
                    Console.WriteLine( "- El script se está ejecutando..." );
                    Console.WriteLine( "- Tiempo transcurrido: {0:mm\\:ss}", _stopwatch.Elapsed );

                    Thread.Sleep( 1000 );

                    continue;
                }

                Console.WriteLine( "- Hotkey actual: {0}", _currentHotkey.ToString( ) );
                Console.WriteLine( "- Script actual: {0}\n", _currentScriptName );

                Console.WriteLine( "[1] Setear hotkey." );
                Console.WriteLine( "[2] Cargar script." );
                Console.WriteLine( "[3] Salir.\n" );

                Thread.Sleep( 1000 );

                if ( !Console.KeyAvailable || _isRunning )
                {
                    continue;
                }

                var input = Console.ReadKey( true ).KeyChar;

                switch ( input )
                {
                    case '1': ReadHotkey( ); break;
                    case '2': ReadScript( ); break;
                }

                if ( input == '3' )
                {
                    _isRunning = false;
                    _isReading = false;

                    Application.Exit( );

                    break;
                }
            }
        }

        private static void ExecuteScript( )
        {
            int scriptSize = _currentScript.Count;
            int instructionCounter = 0;

            while ( _isRunning )
            {
                if ( instructionCounter == scriptSize )
                {
                    instructionCounter = 0;
                }

                _currentScript[ instructionCounter ].Execute( );

                instructionCounter++;
            }
        }

        private static void ReadHotkey( )
        {
            Console.Clear( );

            Console.WriteLine( "- La hotkey sera la que active o desactive el script." );
            Console.WriteLine( "- No elegir una tecla que se utilice en el script.\n" );

            Console.Write( "Presione la tecla que prefiera como hotkey..." );

            _isReading = true;

            while ( _isReading )
            {
                Thread.Sleep( 100 );
            }
        }

        private static void ReadScript( )
        {
            Console.Clear( );

            Console.WriteLine( "- El script debe estar ubicado en la carpeta del programa." );
            Console.WriteLine( "- El script debe ser un archivo de texto (.txt).\n" );

            Console.Write( "Ingrese el nombre del archivo del script: " );

            var input = Console.ReadLine( );
            var file = AppPath + "/" + input + ".txt";

            if ( !File.Exists( file ) )
            {
                Console.Clear( );

                Console.WriteLine( "No se encontró el archivo del script ingresado.");
                Console.WriteLine( "Debe ser un archivo de texto ubicado en el directorio del programa.\n" );

                Console.Write( "Presione una tecla para volver al menu principal..." );

                Console.ReadKey( );

                return;
            }

            var data = File.ReadAllLines( file );
            var script = new List<Instruction>( );

            try
            {
                foreach ( string line in data )
                {
                    if ( string.IsNullOrEmpty( line ) || line.StartsWith( "//" ) )
                    {
                        continue;
                    }

                    script.Add( new Instruction( line ) );
                }
            }
            catch ( Exception ex )
            {
                Console.Clear( );

                Console.WriteLine( "Ocurrió un error mientras se cargaba el script." );
                Console.WriteLine( "Detalle del error: {0}.", ex.Message );

                Console.Write( "Presione una tecla para volver al menu principal..." );

                Console.ReadKey( );
            }

            if ( script.Count == 0 )
            {
                Console.Clear( );

                Console.WriteLine( "No se encontraron instrucciones en el script." );
                Console.WriteLine( "Verifique el script en cuestion y pruebe nuevamente." );

                Console.Write( "Presione una tecla para volver al menu principal..." );

                Console.ReadKey( );

                return;
            }

            _currentScript = new List<Instruction>( script );
            _currentScriptName = Path.GetFileNameWithoutExtension( file );
        }

        private static void DisableQuickEdit( )
        {
            var consoleHandle = NativeMethods.GetStdHandle( STD_INPUT_HANDLE );

            if ( NativeMethods.GetConsoleMode( consoleHandle, out var consoleMode ) )
            {
                NativeMethods.SetConsoleMode( consoleHandle, ( consoleMode & ~ENABLE_QUICK_EDIT ) );
            }
        }

        private static void OnKeyDownEvent( object sender, KeyEventArgs key )
        {
            if ( _isReading )
            {
                _currentHotkey = key.KeyCode;
                _isReading = false;

                return;
            }

            if ( key.KeyCode != _currentHotkey )
            {
                return;
            }

            if ( !_isRunning )
            {
                if ( _currentScript == null )
                {
                    return;
                }

                _stopwatch.Reset( );
                _stopwatch.Start( );

                Task.Run( ExecuteScript );

                _isRunning = true;
            }
            else
            {
                _isRunning = false;
                _stopwatch.Stop( );
            }
        }
    }
}