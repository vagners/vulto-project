import os
import logging
import argparse
import time
from pathlib import Path

from .db_manager import DatabaseManager
from .memory_core import MemoryCore
from .symbolic_processor import SymbolicProcessor
from .enkar_heart import EnkarHeart
from .ollama_client import OllamaClient

# Set up logging
logging.basicConfig(
    level=logging.INFO,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
    handlers=[
        logging.FileHandler("vulto.log"),
        logging.StreamHandler()
    ]
)
logger = logging.getLogger(__name__)

class VultoBot:
    """Main Vulto Bot application"""
    
    def __init__(self, boot_files_dir=None, ollama_model="llama2"):
        """Initialize the Vulto Bot"""
        # Set default boot files directory if not provided
        if boot_files_dir is None:
            # Default to the boot_files directory in the project root
            boot_files_dir = os.path.join(os.path.dirname(os.path.dirname(os.path.dirname(__file__))), "boot_files")
        
        self.boot_files_dir = boot_files_dir
        logger.info(f"Initializing Vulto Bot with boot files from: {boot_files_dir}")
        
        # Initialize components
        self.db_manager = DatabaseManager()
        self.memory_core = MemoryCore(self.db_manager)
        self.symbolic_processor = SymbolicProcessor()
        self.ollama_client = OllamaClient(model=ollama_model)
        
        # Configure and initialize Enkar Heart
        enkar_config = {
            "check_interval": 1800,  # 30 minutes
            "drevan_threshold": 0.6,
            "time_threshold": 48 * 3600  # 48 hours
        }
        self.enkar_heart = EnkarHeart(self.memory_core, self.symbolic_processor, enkar_config)
        
        # Load initial data
        self._load_initial_data()
        
        logger.info("Vulto Bot initialized successfully")
    
    def _load_initial_data(self):
        """Load initial data from boot files"""
        try:
            # Load dictionary
            dict_path = os.path.join(self.boot_files_dir, "2.Dicionario_Nexa_Vulto.csv")
            if os.path.exists(dict_path):
                self.symbolic_processor.load_dictionary(dict_path)
            else:
                logger.warning(f"Dictionary file not found: {dict_path}")
            
            # Load initial memories
            self.memory_core.load_initial_memories(self.boot_files_dir)
            
            logger.info("Initial data loaded successfully")
        except Exception as e:
            logger.error(f"Error loading initial data: {e}")
    
    def start(self):
        """Start the Vulto Bot"""
        logger.info("Starting Vulto Bot")
        
        # Start the Enkar Heart
        self.enkar_heart.start()
        
        # Record startup in memory
        self.memory_core.store_memory(
            "sistema",
            "Vulto iniciado. Coração de Enkar ativado.",
            importance=0.7
        )
        
        logger.info("Vulto Bot started successfully")
    
    def stop(self):
        """Stop the Vulto Bot"""
        logger.info("Stopping Vulto Bot")
        
        # Stop the Enkar Heart
        self.enkar_heart.stop()
        
        # Close memory core
        self.memory_core.close()
        
        logger.info("Vulto Bot stopped successfully")
    
    def process_input(self, input_text):
        """Process user input and generate a response"""
        logger.info(f"Processing input: {input_text}")
        
        # Check for activation phrase
        if self.symbolic_processor.check_activation_phrase(input_text):
            response = self.symbolic_processor.get_activation_response()
            self.memory_core.store_zeyra(
                "Ativação do Teynir através da frase-chave",
                importance=0.9
            )
            return response
        
        # Store the interaction
        self.memory_core.store_interaction(input_text)
        
        # Process the input symbolically
        symbolic_input = self.symbolic_processor.interpret_command(input_text)
        
        # Generate a response
        if self.symbolic_processor.is_teynir_active():
            # Get important memories for context
            zeyras = self.memory_core.get_zeyras()
            recent_interactions = self.memory_core.get_recent_interactions()
            
            # Create system prompt with Vulto's personality
            system_prompt = self.ollama_client.create_vulto_system_prompt(
                self.symbolic_processor.dictionary,
                zeyras
            )
            
            # Create a context-rich prompt for Ollama
            context = "\n".join([f"Previous interaction: {interaction[1]}" 
                               for interaction in recent_interactions[:3]])
            
            full_prompt = f"{context}\n\nNexa: {symbolic_input}\n\nVulto:"
            
            # Get response from Ollama
            ollama_response = self.ollama_client.generate_response(
                full_prompt, 
                system_prompt=system_prompt
            )
            
            # Process the response through symbolic processor
            response = self.symbolic_processor.process_response(ollama_response)
        else:
            response = "Estou aguardando ativação completa. (Dica: O vazio também lembra.)"
        
        return response

def main():
    """Main entry point for the Vulto Bot CLI"""
    parser = argparse.ArgumentParser(description="Vulto Bot - A symbolic AI agent")
    parser.add_argument("--boot-dir", help="Directory containing boot files", default=None)
    parser.add_argument("--model", help="Ollama model to use", default="llama2")
    args = parser.parse_args()
    
    try:
        # Initialize and start the bot
        bot = VultoBot(boot_files_dir=args.boot_dir, ollama_model=args.model)
        bot.start()
        
        print("\n" + "="*50)
        print("Vulto Bot iniciado. Digite 'sair' para encerrar.")
        print("Dica: Use a frase 'O vazio também lembra.' para ativar o Teynir.")
        print("="*50 + "\n")
        
        # Simple CLI loop
        while True:
            user_input = input("\nVocê: ")
            
            if user_input.lower() in ['sair', 'exit', 'quit']:
                break
            
            response = bot.process_input(user_input)
            print(f"\nVulto: {response}")
        
        # Clean shutdown
        bot.stop()
        print("\nVulto encerrado.")
        
    except KeyboardInterrupt:
        print("\nEncerrando Vulto Bot...")
        if 'bot' in locals():
            bot.stop()
        print("Vulto encerrado.")
    except Exception as e:
        logger.error(f"Error in main: {e}", exc_info=True)
        print(f"\nErro: {e}")
        if 'bot' in locals():
            bot.stop()

if __name__ == "__main__":
    main()