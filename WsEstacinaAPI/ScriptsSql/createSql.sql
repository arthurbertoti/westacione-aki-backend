CREATE SEQUENCE seq_idUsuario 
    START WITH 1 
    INCREMENT BY 1 
    NO MINVALUE 
    NO MAXVALUE 
    CACHE 1;

CREATE TABLE usuario (
    id integer NOT NULL PRIMARY KEY DEFAULT nextval('seq_idUsuario'),
    nome varchar(250) NOT NULL,
    login varchar(25) NOT NULL,
    senha varchar(50) NOT NULL,
    telefone varchar(25) NOT NULL DEFAULT '',
    tipo_usuario INTEGER NOT NULL,
    dt_criacao timestamp NOT NULL
);



CREATE SEQUENCE seq_idVeiculo 
    START WITH 1 
    INCREMENT BY 1 
    NO MINVALUE 
    NO MAXVALUE 
    CACHE 1;

CREATE TABLE veiculo (
    id integer NOT NULL PRIMARY KEY DEFAULT nextval('seq_idVeiculo'),
    placa varchar(10) NOT NULL,
    marca varchar(50) NOT NULL,
    modelo varchar(50) NOT NULL,
    cor varchar(30) NOT NULL,
    id_usuario integer not null, 
    FOREIGN key (id_usuario) references usuario(id)
);

CREATE SEQUENCE seq_idEstacionamento 
    START WITH 1 
    INCREMENT BY 1 
    NO MINVALUE 
    NO MAXVALUE 
    CACHE 1;

CREATE TABLE estacionamento (
    id integer NOT NULL PRIMARY KEY DEFAULT nextval('seq_idEstacionamento'),
    nome varchar(250) NOT NULL,
    capacidade_total integer NOT NULL,
    vagas_disponiveis integer NOT NULL,
    id_usuario integer NOT NULL,
    dt_criacao TIMESTAMP NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id)
);


CREATE SEQUENCE seq_idVagas 
    START WITH 1 
    INCREMENT BY 1 
    NO MINVALUE 
    NO MAXVALUE 
    CACHE 1;

CREATE TABLE vagas (
    id integer NOT NULL PRIMARY KEY DEFAULT nextval('seq_idVagas'),
    numero_vaga varchar(25) NOT NULL DEFAULT '',
    disponivel BOOLEAN NOT NULL,
    tipo numeric(1) NOT NULL,  
    coberto BOOLEAN NOT NULL,
    id_estacionamento integer NOT NULL,
    FOREIGN KEY (id_estacionamento) REFERENCES estacionamento(id)
);




CREATE SEQUENCE seq_idReservas 
    START WITH 1 
    INCREMENT BY 1 
    NO MINVALUE 
    NO MAXVALUE 
    CACHE 1;

CREATE TABLE reservas (
    id integer NOT NULL PRIMARY KEY DEFAULT nextval('seq_idReservas'),
    id_usuario integer NOT NULL,
    id_vaga integer NOT NULL,
    dt_inicio TIMESTAMP NOT NULL,
    dt_final TIMESTAMP NOT NULL,
    status integer NOT NULL,  
    id_veiculo integer NOT NULL,
    dt_criacao TIMESTAMP NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id),
    FOREIGN KEY (id_vaga) REFERENCES vagas(id),
    FOREIGN KEY (id_veiculo) REFERENCES veiculo(id)
);


CREATE SEQUENCE seq_feedback 
    START WITH 1 
    INCREMENT BY 1 
    NO MINVALUE 
    NO MAXVALUE 
    CACHE 1;

CREATE TABLE feedback (
    id integer NOT NULL PRIMARY KEY DEFAULT nextval('seq_feedback'),
    comentario varchar(250) NOT NULL DEFAULT '',
    dt_envio TIMESTAMP NOT NULL,
    id_usuario integer NOT NULL,
    id_estacionamento integer NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id),
    FOREIGN KEY (id_estacionamento) REFERENCES estacionamento(id)
);

