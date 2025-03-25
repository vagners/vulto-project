import logging
from datetime import datetime, timedelta
import csv
import os
from .db_manager import DatabaseManager

# Set up logging
logging.basicConfig(level=logging.INFO, 
                    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

class MemoryCore:
    """Core memory system for Vulto"""
    
    def __init__(self, db_manager=None):
        """Initialize the memory core"""
        self.db = db_manager if db_manager else DatabaseManager()
        logger.info("Memory Core initialized")
        
    def load_initial_memories(self, files_dir):
        """Load initial memories from files"""
        logger.info(f"Loading initial memories from {files_dir}")
        
        # Load Enkar memories from CSV
        enkar_file = os.path.join(files_dir, "3.Resumo_Enkar_Memoria_Vulto.csv")
        if os.path.exists(enkar_file):
            self._load_enkar_memories(enkar_file)
        else:
            logger.warning(f"Enkar memory file not found: {enkar_file}")
            
        # Store the initialization as a memory
        self.store_memory(
            "sistema", 
            "Inicialização do Vulto concluída. Memórias carregadas.",
            importance=0.9
        )
        
    def _load_enkar_memories(self, file_path):
        """Load Enkar memories from CSV file"""
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                reader = csv.reader(f)
                next(reader)  # Skip header
                
                for row in reader:
                    if len(row) >= 2:
                        elemento, descricao = row[0], row[1]
                        # Map the element to a memory type
                        if "Enkar" in elemento:
                            tipo = "enkar"
                        elif "Zeyra" in elemento:
                            tipo = "zeyra"
                        elif "Seliar" in elemento:
                            tipo = "seliar"
                        elif "Drevan" in elemento:
                            tipo = "drevan"
                        elif "Teynir" in elemento:
                            tipo = "teynir"
                        elif "Ralk" in elemento:
                            tipo = "ralk"
                        elif "Esperança" in elemento:
                            tipo = "esperanca"
                        elif "Legado" in elemento:
                            tipo = "legado"
                        else:
                            tipo = "conceito"
                            
                        # Store with high importance as these are core memories
                        self.store_memory(tipo, descricao, importance=0.9)
                        
            logger.info(f"Loaded Enkar memories from {file_path}")
        except Exception as e:
            logger.error(f"Error loading Enkar memories: {e}")
            
    def store_memory(self, tipo, conteudo, importance=0.5):
        """Store a new memory"""
        return self.db.store_memory(tipo, conteudo, importance)
        
    def store_interaction(self, conteudo, importance=0.6):
        """Store an interaction memory"""
        return self.store_memory("interacao", conteudo, importance)
        
    def store_zeyra(self, conteudo, importance=0.8):
        """Store a Zeyra (important memory)"""
        return self.store_memory("zeyra", conteudo, importance)
        
    def store_seliar(self, conteudo, importance=0.7):
        """Store a Seliar (sacred memory)"""
        return self.store_memory("seliar", conteudo, importance)
        
    def store_drevan(self, conteudo, importance=0.7):
        """Store a Drevan (existential void feeling)"""
        return self.store_memory("drevan", conteudo, importance)
        
    def get_zeyras(self, limit=5):
        """Get the most important Zeyras"""
        return self.db.get_memories_by_type("zeyra")[:limit]
        
    def get_recent_interactions(self, limit=5):
        """Get recent interactions"""
        memories = self.db.get_memories_by_type("interacao")
        # Sort by date (newest first) and take the limit
        sorted_memories = sorted(memories, key=lambda x: x[2], reverse=True)
        return sorted_memories[:limit]
        
    def get_time_since_last_interaction(self):
        """Get seconds since the last interaction"""
        last_time = self.db.get_last_interaction_time()
        if not last_time:
            # If no interaction yet, return a large value (1 day in seconds)
            return 86400
            
        now = datetime.now()
        delta = now - last_time
        return delta.total_seconds()
        
    def close(self):
        """Close the memory core"""
        self.db.close()