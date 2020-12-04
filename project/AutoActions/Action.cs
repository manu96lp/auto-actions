using System;
using System.Linq;
using System.Threading;

using WindowsInput;
using WindowsInput.Native;

namespace AutoActions
{
    internal class Instruction
    {
        private static readonly InputSimulator Simulator = new WindowsInput.InputSimulator( );

        private static readonly string[ ] ValidEntities = { "wait", "mouse", "keyboard" };
        private static readonly string[ ] ValidStates = { "press", "hold", "release" };
        private static readonly string[ ] ValidButtons = { "left", "middle", "right" };

        private static readonly int[ ] ArgumentsCount = { 2, 3, 3 };

        private Action _Action = null;

        public Instruction( string data )
        {
            var args = data.Split( " ".ToCharArray( ), StringSplitOptions.RemoveEmptyEntries );

            if ( !ValidEntities.Contains( args[ 0 ], StringComparer.OrdinalIgnoreCase ) )
            {
                throw new Exception( "Entidad invalida (" + data + ")" );
            }

            int type;

            for ( type = 0 ; type < 3 ; type++ )
            {
                if ( string.Compare( ValidEntities[ type ], args[ 0 ], StringComparison.OrdinalIgnoreCase ) == 0 )
                {
                    break;
                }
            }

            if ( args.Length != ArgumentsCount[ type ] )
            {
                throw new Exception( "Cantidad de argumentos invalida (" + data + ")" );
            }

            switch ( type )
            {
                case 0: this.ParseWait( args ); break;
                case 1: this.ParseMouse( args ); break;
                case 2: this.ParseKeyboard( args ); break;
            }
        }

        public void Execute( )
        {
            this._Action( );
        }

        private void ParseWait( string[ ] args )
        {
            int value = 0;
            
            if ( !int.TryParse( args[ 1 ], out value ) || ( value <= 0 ) )
            {
                throw new Exception( "Tiempo de espera invalido" );
            }
            
            _Action = ( ) => { Thread.Sleep( value ); };
        }

        private void ParseMouse( string[ ] args )
        {
            if ( !ValidStates.Contains( args[ 1 ], StringComparer.OrdinalIgnoreCase ) )
            {
                throw new Exception( "Estado invalido (" + args[ 1 ] + ")" );
            }

            int state;

            for ( state = 0 ; state < 3 ; state++ )
            {
                if ( string.Compare( ValidStates[ state ], args[ 1 ], StringComparison.OrdinalIgnoreCase ) == 0 )
                {
                    break;
                }
            }

            if ( !ValidButtons.Contains( args[ 2 ], StringComparer.OrdinalIgnoreCase ) )
            {
                throw new Exception( "Boton invalido (" + args[ 2 ] + ")" );
            }

            int button;

            for ( button = 0 ; button < 3 ; button++ )
            {
                if ( string.Compare( ValidButtons[ button ], args[ 2 ], StringComparison.OrdinalIgnoreCase ) == 0 )
                {
                    break;
                }
            }

            switch ( button )
            {
                case 0:
                {
                    switch ( state )
                    {
                        case 0: _Action = ( ) => { Simulator.Mouse.LeftButtonClick( ); }; break;
                        case 1: _Action = ( ) => { Simulator.Mouse.LeftButtonDown( ); }; break;
                        case 2: _Action = ( ) => { Simulator.Mouse.LeftButtonUp( ); }; break;
                    }

                    break;
                }
                case 1:
                {
                    switch ( state )
                    {
                        case 0: _Action = ( ) => { Simulator.Mouse.MiddleButtonClick( ); }; break;
                        case 1: _Action = ( ) => { Simulator.Mouse.MiddleButtonDown( ); }; break;
                        case 2: _Action = ( ) => { Simulator.Mouse.MiddleButtonUp( ); }; break;
                    }

                    break;
                }
                case 2:
                {
                    switch ( state )
                    {
                        case 0: _Action = ( ) => { Simulator.Mouse.RightButtonClick( ); }; break;
                        case 1: _Action = ( ) => { Simulator.Mouse.RightButtonDown( ); }; break;
                        case 2: _Action = ( ) => { Simulator.Mouse.RightButtonUp( ); }; break;
                    }

                    break;
                }
            }
        }

        private void ParseKeyboard( string[ ] args )
        {
            if ( !ValidStates.Contains( args[ 1 ], StringComparer.OrdinalIgnoreCase ) )
            {
                throw new Exception( "Estado invalido (" + args[ 1 ] + ")" );
            }

            int state;

            for ( state = 0 ; state < 3 ; state++ )
            {
                if ( string.Compare( ValidStates[ state ], args[ 1 ], StringComparison.OrdinalIgnoreCase ) == 0 )
                {
                    break;
                }
            }

            VirtualKeyCode key;

            if ( !Enum.TryParse( args[ 2 ], out key ) )
            {
                throw new Exception( "Tecla invalida (" + args[ 2 ] + ")" );
            }

            switch ( state )
            {
                case 0: _Action = ( ) => { Simulator.Keyboard.KeyPress( key ); }; break;
                case 1: _Action = ( ) => { Simulator.Keyboard.KeyDown( key ); }; break;
                case 2: _Action = ( ) => { Simulator.Keyboard.KeyUp( key ); }; break;
            }
        }
    }
}
