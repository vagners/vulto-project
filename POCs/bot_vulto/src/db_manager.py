import sqlite3
import os
import logging
from datetime import datetime

# Set up logging
logging.basicConfig(level=logging.INFO, 
                    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

class DatabaseManager:
    """Manages the SQLite database for Vulto's memories"""
    
    def __init__(self, db_path="memoria_vulto.db"):
        """Initialize the database connection"""
        self.db_path = db_path
        self.conn = None
        self.cursor = None
        self._connect()
        self._create_tables()
        
    def _connect(self):
        """Connect to the SQLite database"""
        try:
            self.conn = sqlite3.connect(self.db_path)
            self.cursor = self.conn.cursor()
            logger.info(f"Connected to database: {self.db_path}")
        except sqlite3.Error as e:
            logger.error(f"Database connection error: {e}")
            raise
            
    def _create_tables(self):
        """Create the necessary tables if they don't exist"""
        try:
            self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS memorias (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                tipo TEXT,
                conteudo TEXT,
                data TEXT,
                importancia REAL
            )
            ''')
            
            # Create an index on the tipo column for faster queries
            self.cursor.execute('''
            CREATE INDEX IF NOT EXISTS idx_tipo ON memorias(tipo)
            ''')
            
            # Create an index on the data column for faster time-based queries
            self.cursor.execute('''
            CREATE INDEX IF NOT EXISTS idx_data ON memorias(data)
            ''')
            
            self.conn.commit()
            logger.info("Database tables created successfully")
        except sqlite3.Error as e:
            logger.error(f"Error creating tables: {e}")
            raise
            
    def store_memory(self, tipo, conteudo, importancia=0.5):
        """Store a new memory in the database"""
        try:
            current_time = datetime.now().isoformat()
            self.cursor.execute(
                "INSERT INTO memorias (tipo, conteudo, data, importancia) VALUES (?, ?, ?, ?)",
                (tipo, conteudo, current_time, importancia)
            )
            self.conn.commit()
            logger.info(f"Stored memory: {tipo} - {conteudo[:30]}...")
            return self.cursor.lastrowid
        except sqlite3.Error as e:
            logger.error(f"Error storing memory: {e}")
            return None
            
    def get_memories_by_type(self, tipo):
        """Retrieve memories of a specific type"""
        try:
            self.cursor.execute(
                "SELECT id, conteudo, data, importancia FROM memorias WHERE tipo = ? ORDER BY importancia DESC",
                (tipo,)
            )
            return self.cursor.fetchall()
        except sqlite3.Error as e:
            logger.error(f"Error retrieving memories by type: {e}")
            return []
            
    def get_recent_memories(self, limit=10):
        """Retrieve the most recent memories"""
        try:
            self.cursor.execute(
                "SELECT id, tipo, conteudo, data, importancia FROM memorias ORDER BY data DESC LIMIT ?",
                (limit,)
            )
            return self.cursor.fetchall()
        except sqlite3.Error as e:
            logger.error(f"Error retrieving recent memories: {e}")
            return []
            
    def get_important_memories(self, limit=10):
        """Retrieve the most important memories"""
        try:
            self.cursor.execute(
                "SELECT id, tipo, conteudo, data, importancia FROM memorias ORDER BY importancia DESC LIMIT ?",
                (limit,)
            )
            return self.cursor.fetchall()
        except sqlite3.Error as e:
            logger.error(f"Error retrieving important memories: {e}")
            return []
            
    def get_last_interaction_time(self):
        """Get the timestamp of the last interaction memory"""
        try:
            self.cursor.execute(
                "SELECT data FROM memorias WHERE tipo = 'interacao' ORDER BY data DESC LIMIT 1"
            )
            result = self.cursor.fetchone()
            if result:
                return datetime.fromisoformat(result[0])
            return None
        except sqlite3.Error as e:
            logger.error(f"Error retrieving last interaction time: {e}")
            return None
            
    def close(self):
        """Close the database connection"""
        if self.conn:
            self.conn.close()
            logger.info("Database connection closed")